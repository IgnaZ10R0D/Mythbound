using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity;

    private const float MaxLifeTime = 10f;
    private float _lifeTime = 0f;

    private float currentTimeFactor = 1f;
    private SpriteRenderer spriteRenderer;

    [Header("VFX")]
    [SerializeField] private ParticleSystem despawnEffect; 
    // 👆 IMPORTANTE: ahora es ParticleSystem prefab

    public Transform Target { get; private set; }

    // =========================================================
    // Behaviour system
    // =========================================================
    private BulletBehaviour _behaviourProfile;
    private BulletActivationCondition _conditionProfile;

    private BulletBehaviourInstance _behaviourInstance;
    private BulletActivationConditionInstance _conditionInstance;

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

        _behaviourProfile = null;
        _conditionProfile = null;
        _behaviourInstance = null;
        _conditionInstance = null;
        Target = null;
    }

    private void OnTimeWarpChanged(float newFactor)
    {
        currentTimeFactor = newFactor;
    }

    // =========================================================
    // Injection
    // =========================================================
    public void InjectBehaviour(
        BulletBehaviour behaviour,
        BulletActivationCondition condition)
    {
        _behaviourProfile = behaviour;
        _conditionProfile = condition;

        _behaviourInstance = null;
        _conditionInstance = null;

        if (_behaviourProfile == null)
            return;

        if (_conditionProfile != null)
        {
            _conditionInstance = _conditionProfile.CreateInstance();
            _conditionInstance.Initialize(this);
        }
        else
        {
            ActivateBehaviour();
        }
    }

    private void ActivateBehaviour()
    {
        if (_behaviourInstance != null || _behaviourProfile == null)
            return;

        _behaviourInstance = _behaviourProfile.CreateInstance();
        _behaviourInstance.Initialize(this);
    }

    public void InjectTarget(Transform target)
    {
        Target = target;
    }

    public void ApplyVisual(RadialShotSettings settings)
    {
        if (spriteRenderer != null && settings.BulletSprite != null)
            spriteRenderer.sprite = settings.BulletSprite;

        transform.localScale = settings.BulletScale;
    }

    // =========================================================
    // Update
    // =========================================================
    private void Update()
    {
        transform.position += (Vector3)(Velocity * currentTimeFactor * Time.deltaTime);

        if (_conditionInstance != null && _behaviourInstance == null)
        {
            _conditionInstance.Tick(Time.deltaTime);

            if (_conditionInstance.IsActive)
                ActivateBehaviour();
        }

        if (_behaviourInstance != null)
            _behaviourInstance.Tick(Time.deltaTime);

        _lifeTime += Time.deltaTime;
        if (_lifeTime >= MaxLifeTime)
            Despawn();
    }

    // =========================================================
    // DESPAWN
    // =========================================================
    public void Despawn()
    {
        Vector3 pos = transform.position;

        if (despawnEffect != null && VFXPool.Instance != null)
        {
            // 🔥 AQUÍ está lo importante
            ExplosionVFX vfx = VFXPool.Instance.Get(despawnEffect);
            vfx.Play(pos);
        }

        gameObject.SetActive(false);
    }
}

