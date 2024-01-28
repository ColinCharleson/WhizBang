
using UnityEngine;
using UnityEngine.AI;


public class EnemyAi : MonoBehaviour
{
    [SerializeField] private int scoreValue;
    public NavMeshAgent agent;

    public Transform player;
    public Animator animator;

    public Transform bulletSpawnPoint;

    public LayerMask whatIsGround, whatIsPlayer;

    public delegate void EnemyDeathHandler();

    public event EnemyDeathHandler OnDeath;

    public float health;

    bool isAlive = true;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //Particles
    public ParticleSystem hit;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Sounds
    public AudioSource hitSound, walkSound;
    private float nextWalkSoundTime;
    private float minTimeBetweenWalkSounds = 15f;
    private float maxTimeBetweenWalkSounds = 30f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (isAlive)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    private void Patroling()
    {
        animator.SetBool("Attacking", false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (isAlive)
        {
            if (Time.time >= nextWalkSoundTime)
            {
                if (walkSound != null)
                {
                    walkSound.Play();
                }
                nextWalkSoundTime = Time.time + Random.Range(minTimeBetweenWalkSounds, maxTimeBetweenWalkSounds);
            }
        }
        animator.SetBool("Attacking", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animator.SetBool("Attacking", true);
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            ///
            if (isAlive)
            {
                Rigidbody rb = Instantiate(projectile, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        
                ///End of attack code
                Destroy(rb.gameObject, 2f);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        hit.Play();

        if (hitSound != null)
        {
            hitSound.Play();
        }

        if (health <= 0 && isAlive)
        {
            health = 0;
            isAlive = false;
            OnDeath();
            animator.SetTrigger("Die");
            Invoke(nameof(DestroyEnemy), 0.5f);
            ScoreSystem.instance.UpdateScore(scoreValue);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
