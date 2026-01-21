using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Velocity;

    private const float MaxLifeTime = 10f;
    private float _lifeTime = 0f;

    private float currentTimeFactor = 1f;
    private SpriteRenderer spriteRenderer;
    
    public Transform Target { get; private set; }

    // =========================================================
    // Behaviour system (profiles + runtime instances)
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

        // --- Total cleaning for the pool ---
        _behaviourProfile = null;
        _conditionProfile = null;

        _behaviourInstance = null;
        _conditionInstance = null;
    }

    private void OnTimeWarpChanged(float newFactor)
    {
        currentTimeFactor = newFactor;
    }

    // =========================================================
    // Data injection (ShotAttack)
    // =========================================================
    public void InjectBehaviour(
        BulletBehaviour behaviour,
        BulletActivationCondition condition)
    {
        _behaviourProfile = behaviour;
        _conditionProfile = condition;

        _behaviourInstance = null;
        _conditionInstance = null;

        // Normal bullet
        if (_behaviourProfile == null)
            return;

        // Wait until condition is completed before applying
        if (_conditionProfile != null)
        {
            _conditionInstance = _conditionProfile.CreateInstance();
            _conditionInstance.Initialize(this);
        }
        else
        {
            // No condition = activate immediately
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

    public void ApplyVisual(RadialShotSettings settings)
    {
        if (spriteRenderer != null && settings.BulletSprite != null)
            spriteRenderer.sprite = settings.BulletSprite;

        transform.localScale = settings.BulletScale;
    }

    private void Update()
    {
        // --- Base movement ---
        transform.position += (Vector3)(Velocity * currentTimeFactor * Time.deltaTime);

        // --- Establish conditions ---
        if (_conditionInstance != null && _behaviourInstance == null)
        {
            _conditionInstance.Tick(Time.deltaTime);

            if (_conditionInstance.IsActive)
            {
                ActivateBehaviour();
            }
        }

        // --- Execute active behaviour ---
        if (_behaviourInstance != null)
        {
            _behaviourInstance.Tick(Time.deltaTime);
        }

        // --- Lifetime ---
        _lifeTime += Time.deltaTime;
        if (_lifeTime >= MaxLifeTime)
            Disable();
    }

    private void Disable()
    {
        _lifeTime = 0f;
        gameObject.SetActive(false);
    }
    public void InjectTarget(Transform target)
    {
        Target = target;
    }

}

