using UnityEngine;

public class SimpleBullet : MonoBehaviour, IEnemyBullet
{
    public float Duration { get; set; }
    public float MovementSpeed { get; set; } 

    private float baseMovementSpeed;
    private float timeAlive;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private BulletConfig config;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyConfig(BulletConfig config)
    {
        Duration = config.duration;
        baseMovementSpeed = config.movementSpeed;

        if (config.bulletSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = config.bulletSprite;
        }
    }

    void Start()
    {
        timeAlive = 0f;
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
        float timeFactor = (TimeManager.Instance != null ? TimeManager.Instance.TimeSlow : 1f);
        float speed = baseMovementSpeed * timeFactor;
        MovementSpeed = speed;

        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}




