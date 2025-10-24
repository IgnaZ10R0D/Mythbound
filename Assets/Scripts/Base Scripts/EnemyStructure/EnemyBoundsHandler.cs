using UnityEngine;

public class EnemyBoundsHandler : MonoBehaviour
{
    private Collider2D enemyCollider;
    private MonoBehaviour[] attackScripts; 
    private bool isOutOfBounds = false;

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

        Vector3 screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

        if (transform.position.x < -screenBounds.x || transform.position.x > screenBounds.x ||
            transform.position.y < -screenBounds.y || transform.position.y > screenBounds.y)
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
    }

    private void HandleOutOfBounds()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        foreach (MonoBehaviour script in attackScripts)
        {
            if (script is IAttack)
            {
                script.enabled = false;
            }
        }
    }

    private void HandleInBounds()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }

        foreach (MonoBehaviour script in attackScripts)
        {
            if (script is IAttack)
            {
                script.enabled = true;
            }
        }
    }
}

