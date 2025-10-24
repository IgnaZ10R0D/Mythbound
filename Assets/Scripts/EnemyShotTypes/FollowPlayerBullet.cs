using UnityEngine;

public class FollowPlayerBullet : MonoBehaviour, IEnemyBullet
{
    public float Duration { get; set; }
    public float MovementSpeed { get; set; }
    public float DelayBeforeTargeting { get; set; } = 0f; 
    public float FollowSeconds { get; set; }

    private GameObject player;
    private float timeAlive;
    private bool followingPlayer = true;
    private Vector3 lastDirection;
    private float baseSpeed;

    [SerializeField] private BulletConfig config;
    private SpriteRenderer spriteRenderer;

    public void ApplyConfig(BulletConfig config)
    {
        Duration = config.duration;
        baseSpeed = config.movementSpeed;
        FollowSeconds = config.followSeconds;

        UpdateMovementSpeed();

        if (config.bulletSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = config.bulletSprite;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        ApplyConfig(config); 
        timeAlive = 0f;

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged += OnTimeWarpChanged;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged -= OnTimeWarpChanged;
    }

    private void OnTimeWarpChanged(float timeFactor)
    {
        UpdateMovementSpeed();
    }

    private void UpdateMovementSpeed()
    {
        MovementSpeed = baseSpeed * (TimeManager.Instance != null ? TimeManager.Instance.TimeSlow : 1f);
    }

    void Update()
    {
        BulletMovement();
        timeAlive += Time.deltaTime;

        if (timeAlive >= Duration)
        {
            Destroy(gameObject);
        }
    }

    public void BulletMovement()
    {
        if (followingPlayer && player != null)
        {
            if (timeAlive < FollowSeconds)
            {
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                lastDirection = directionToPlayer;
                transform.Translate(lastDirection * MovementSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                followingPlayer = false;
                transform.Translate(lastDirection * MovementSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.Translate(lastDirection * MovementSpeed * Time.deltaTime, Space.World);
        }
    }
}



