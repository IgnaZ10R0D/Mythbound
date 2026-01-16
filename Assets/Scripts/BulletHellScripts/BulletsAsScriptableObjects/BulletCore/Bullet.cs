using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity;

    private const float MaxLifeTime = 3f;
    private float _lifeTime = 0f;

    private float currentTimeFactor = 1f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _lifeTime = 0f;

        if (TimeManager.Instance != null)
        {
            currentTimeFactor = TimeManager.Instance.TimeSlow;
            TimeManager.Instance.OnTimeWarpChanged += OnTimeWarpChanged;
        }
        else
        {
            currentTimeFactor = 1f;
        }
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged -= OnTimeWarpChanged;
    }

    private void OnTimeWarpChanged(float newFactor)
    {
        currentTimeFactor = newFactor;
    }

    public void ApplyVisual(RadialShotSettings settings)
    {
        if (spriteRenderer != null && settings.BulletSprite != null)
            spriteRenderer.sprite = settings.BulletSprite;

        transform.localScale = settings.BulletScale;
    }

    private void Update()
    {
        transform.position += (Vector3)(Velocity * currentTimeFactor * Time.deltaTime);

        _lifeTime += Time.deltaTime;
        if (_lifeTime >= MaxLifeTime)
            Disable();
    }

    private void Disable()
    {
        _lifeTime = 0f;
        gameObject.SetActive(false);
    }
}



