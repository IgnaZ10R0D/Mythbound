using UnityEngine;

public class PowerItem : MonoBehaviour
{
    [SerializeField] private int pointsToAdd = 50;
    [SerializeField] private float upwardForce = 5f;
    private PointManager pointManager;

    private void Start()
    {
        pointManager = FindAnyObjectByType<PointManager>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);
        }

        SetupCollisionIgnore();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent("Player") && pointManager != null)
        {
            pointManager.AddPoints(pointsToAdd);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckIfOutOfCameraBounds();
    }

    private void CheckIfOutOfCameraBounds()
    {
        Camera camera = Camera.main;
        if (camera == null) return;

        Vector3 screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

        if (transform.position.x < -screenBounds.x ||
            transform.position.x > screenBounds.x ||
            transform.position.y < -screenBounds.y)
        {
            Destroy(gameObject);
        }
    }

    private void SetupCollisionIgnore()
    {
        Collider2D itemCollider = GetComponent<Collider2D>();
        if (itemCollider == null)
        {
            Debug.LogWarning("PowerItem: No se encontró un Collider2D en este objeto.");
            return;
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(itemCollider, enemyCollider, true);
                Physics2D.IgnoreCollision(enemyCollider, itemCollider, true); 
            }
        }
    }
}






