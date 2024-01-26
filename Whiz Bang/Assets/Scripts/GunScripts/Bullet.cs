using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 2;


    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            // Get the EnemyAi script from the enemy GameObject
            EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();

            // Check if the EnemyAi script is found
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damage);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}