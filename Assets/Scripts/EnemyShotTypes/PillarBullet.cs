using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PillarBullet : MonoBehaviour, IEnemyBullet
{
    public float Duration { get; set; }
    public float MovementSpeed { get; set; }

    private float baseMovementSpeed;
    private float timer;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    private BulletConfig config;
    private Vector3 initialScale;
    private bool expanding = true;

    public void ApplyConfig(BulletConfig config)
    {
        this.config = config;
        Duration = config.duration;
        baseMovementSpeed = config.movementSpeed;

        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (sr != null && config.bulletSprite != null)
        {
            sr.sprite = config.bulletSprite;
        }

        initialScale = transform.localScale;
        transform.localScale = new Vector3(initialScale.x, 0.1f, initialScale.z);

        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.simulated = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        boxCollider.isTrigger = false;
        UpdateCollider();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < Duration)
        {
            ExpandPillar();
            BulletMovement(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BulletMovement()
    {
        float timeFactor = (TimeManager.Instance != null ? TimeManager.Instance.TimeSlow : 1f);
        float speed = baseMovementSpeed * timeFactor;

        MovementSpeed = speed;

        if (speed != 0)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    private void ExpandPillar()
    {
        if (!expanding) return;

        float timeFactor = (TimeManager.Instance != null ? TimeManager.Instance.TimeSlow : 1f);
        float growth = config.expandSpeed * timeFactor * Time.deltaTime;

        switch (config.expandDirection)
        {
            case BulletConfig.PillarExpandDirection.Down:
                transform.localScale += new Vector3(0, growth, 0);
                transform.position += Vector3.down * (growth / 2f);
                break;

            case BulletConfig.PillarExpandDirection.Up:
                transform.localScale += new Vector3(0, growth, 0);
                transform.position += Vector3.up * (growth / 2f);
                break;

            case BulletConfig.PillarExpandDirection.Both:
                transform.localScale += new Vector3(0, growth, 0);
                break;
        }

        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (sr == null || boxCollider == null) return;

        Bounds bounds = sr.bounds;
        boxCollider.size = new Vector2(bounds.size.x / transform.lossyScale.x, bounds.size.y / transform.lossyScale.y);
        boxCollider.offset = Vector2.zero;
    }
}
