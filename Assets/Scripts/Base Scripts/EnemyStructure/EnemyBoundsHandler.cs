using UnityEngine;

public class EnemyBoundsHandler : MonoBehaviour
{
    private Collider2D enemyCollider;
    private MonoBehaviour[] attackScripts;

    private bool isOutOfBounds = false;

    [Header("Destroy Settings")]
    [SerializeField] private float destroyMargin = 5f;

    private void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        attackScripts = GetComponents<MonoBehaviour>();
    }

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        Camera camera = Camera.main;
        if (camera == null) return;

        Vector3 screenBounds = camera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, camera.transform.position.z)
        );

        float minX = -screenBounds.x;
        float maxX = screenBounds.x;
        float minY = -screenBounds.y;
        float maxY = screenBounds.y;

        Vector3 pos = transform.position;

        bool outsideCamera =
            pos.x < minX || pos.x > maxX ||
            pos.y < minY || pos.y > maxY;

        if (outsideCamera)
        {
            if (!isOutOfBounds)
            {
                HandleOutOfBounds();
                isOutOfBounds = true;
            }
        }
        else
        {
            if (isOutOfBounds)
            {
                HandleInBounds();
                isOutOfBounds = false;
            }
        }

        CheckDestroyBounds(pos, minX, maxX, minY, maxY);
    }

    private void CheckDestroyBounds(Vector3 pos, float minX, float maxX, float minY, float maxY)
    {
        if (pos.x < minX - destroyMargin ||
            pos.x > maxX + destroyMargin ||
            pos.y < minY - destroyMargin ||
            pos.y > maxY + destroyMargin)
        {
            Destroy(gameObject);
        }
    }

    private void HandleOutOfBounds()
    {
        if (enemyCollider != null)
            enemyCollider.enabled = false;
    }

    private void HandleInBounds()
    {
        if (enemyCollider != null)
            enemyCollider.enabled = true;
    }
}

