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

            // Deal damage to the enemy
            if (collision.gameObject.GetComponent<EnemyAi>())
                collision.gameObject.GetComponent<EnemyAi>().TakeDamage(damage);
            if (collision.gameObject.GetComponent<MeleeAI>())
                collision.gameObject.GetComponent<MeleeAI>().TakeDamage(damage);

            // Destroy the bullet
            Destroy(gameObject);
        }
        
        Destroy(gameObject);
    }
}