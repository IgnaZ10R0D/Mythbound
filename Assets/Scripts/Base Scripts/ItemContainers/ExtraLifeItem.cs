using UnityEngine;

public class ExtraLifeItem : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent("Player") && playerHealth != null)
        {
            playerHealth.LifeGet();
            Destroy(gameObject);
        }
    }
}


