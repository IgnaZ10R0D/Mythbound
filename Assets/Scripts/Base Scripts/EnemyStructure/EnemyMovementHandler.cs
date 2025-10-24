using UnityEngine;
using System.Collections;

public sealed class EnemyMovementHandler : MonoBehaviour
{
    [SerializeField] public static float minX = -10f;
    [SerializeField] public static float maxX = 10f;
    [SerializeField] public static float minY = -10f;
    [SerializeField] public static float maxY = 10f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float delayBeforeMove;
    [SerializeField] private bool isMoving;

    private float baseSpeed;
    private float delayTimer = 0f;
    private bool canMove = false;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimationController animationController;

    private void Start()
    {
        baseSpeed = speed;

        lastPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animationController = GetComponent<EnemyAnimationController>();

        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        StartCoroutine(WaitForTimeManager());
    }

    private IEnumerator WaitForTimeManager()
    {
        while (TimeManager.Instance == null)
        {
            yield return null; 
        }

        UpdateSpeedMultiplier(TimeManager.Instance.TimeSlow);

        TimeManager.Instance.OnTimeWarpChanged += UpdateSpeedMultiplier;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeWarpChanged -= UpdateSpeedMultiplier;
        }
    }

    private void UpdateSpeedMultiplier(float timeFactor)
    {
        speed = baseSpeed * timeFactor;
    }

    private void Update()
    {
        delayTimer += Time.deltaTime;
        if (!canMove && delayTimer >= delayBeforeMove)
        {
            canMove = true;
            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                collider.enabled = true;
            }
        }

        if (canMove)
        {
            foreach (IMovement movement in GetComponents<IMovement>())
            {
                if (movement.enemyMovementScript == null)
                {
                    movement.enemyMovementScript = this;
                }

                movement.Move(transform, speed);
            }
        }

        Vector3 deltaPosition = transform.position - lastPosition;
        isMoving = (deltaPosition != Vector3.zero);

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = deltaPosition.x > 0;
        }

        animationController?.SetMoving(isMoving);

        lastPosition = transform.position;
    }

    public float GetSpeed()
    {
        return speed;
    }
}


