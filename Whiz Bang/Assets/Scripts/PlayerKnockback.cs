using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Penis");
            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
