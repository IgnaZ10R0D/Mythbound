using UnityEngine;
using System.Collections;

public class SpawnTargetBullet : MonoBehaviour, IEnemyBullet
{
    public float Duration { get; set; }
    public float MovementSpeed { get; set; }
    public float DelayBeforeTargeting { get; set; }

    private float baseMovementSpeed;
    private Vector3 direction;
    private bool targetPositionSet = false;
    private float timeAlive;

    private Player player;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private BulletConfig config;

    public void ApplyConfig(BulletConfig config)
    {
        Duration = config.duration;
        baseMovementSpeed = config.movementSpeed;
        DelayBeforeTargeting = config.delayBeforeTargeting;

        if (config.bulletSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = config.bulletSprite;
        }

        UpdateMovementSpeed();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogWarning("SpawnTargetBullet: No se encontró un objeto Player en la escena.");
        }

        StartCoroutine(TargetPlayerAfterDelay());

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeWarpChanged += OnTimeWarpChanged;
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeWarpChanged -= OnTimeWarpChanged;
        }
    }

    private void OnTimeWarpChanged(float timeFactor)
    {
        UpdateMovementSpeed();
    }

    private void UpdateMovementSpeed()
    {
        MovementSpeed = baseMovementSpeed * (TimeManager.Instance != null ? TimeManager.Instance.TimeSlow : 1f);
    }

    private IEnumerator TargetPlayerAfterDelay()
    {
        yield return new WaitForSeconds(DelayBeforeTargeting);

        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            targetPositionSet = true;
        }
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive >= Duration)
        {
            Destroy(gameObject);
            return;
        }

        if (targetPositionSet)
        {
            BulletMovement();
        }
    }

    public void BulletMovement()
    {
        transform.position += direction * MovementSpeed * Time.deltaTime;
    }
}




