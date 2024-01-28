using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    [SerializeField] private int damage;
    [SerializeField] private int scoreValue;
    private bool isAlive = true;

    [Header("AI")]
    [SerializeField] private float timeBetweenAttacks;  
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float walkPointRange;
    private bool canAttack = true;

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [Header("Other")]
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    public delegate void EnemyDeathHandler();

    public event EnemyDeathHandler OnDeath;

    // patrolling
    private Vector3 walkPoint;
    private bool walkPointSet;

    //particles
    public ParticleSystem hit;

    //Sounds
    public AudioSource hitSound, walkSound, attackNoise;
    private float nextWalkSoundTime;
    private float minTimeBetweenWalkSounds = 15f;
    private float maxTimeBetweenWalkSounds = 30f;

    // states
    private bool alreadyAttacked;
    private bool playerInSightRange, playerInAttackRange;

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
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            
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
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        if (!alreadyAttacked && canAttack)
        {
            ///Attack code here
            if (isAlive)
            {
                attackNoise.Play();
                StartCoroutine(Swing());
            }
        }
    }
    IEnumerator Swing()
    {
        canAttack = false;
        animator.SetBool("Attacking", true);
        if (Vector3.Distance(player.transform.position, this.transform.position) < attackRange+ 1f)
        {
            player.gameObject.GetComponent<PlayerMovement>().health -= damage;
        }

        yield return new WaitForSeconds(1);
        Patroling();
        yield return new WaitForSeconds(2);
        canAttack = true;
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
