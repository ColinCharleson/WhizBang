using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 2;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit!");
            // Get the EnemyAi script from the enemy GameObject
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            // Check if the EnemyAi script is found
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}