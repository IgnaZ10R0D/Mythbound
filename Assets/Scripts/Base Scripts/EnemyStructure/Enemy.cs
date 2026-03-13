using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float[] _health = { 100f };
    public int scoreToAdd = 100;

    private int _currentHealthIndex = 0;
    public int HealthIndex;

    [SerializeField] private GameObject dropItem;
    [SerializeField] private ParticleSystem particlePrefab;

    [Header("Damage Flash")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private int flashFrames = 1;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    [Header("Audio Settings")]
    [SerializeField] private string[] takeDamageKeys = { "TakeDamage" };
    [SerializeField] private string[] dieKeys = { "Die" };

    private int currentTakeDamageIndex = 0;
    private int currentDieIndex = 0;

    private bool isDead = false;

    public bool IsDead => isDead;

    public float CurrentHealth
    {
        get
        {
            if (_currentHealthIndex >= _health.Length)
                return 0f;

            return _health[_currentHealthIndex];
        }
    }

    public float MaxHealth
    {
        get
        {
            if (_currentHealthIndex >= _health.Length)
                return 0f;

            return _health[_currentHealthIndex];
        }
    }

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        HealthIndex = _currentHealthIndex;
    }

    void Update()
    {
        if (HealthIndex != _currentHealthIndex)
            HealthIndex = _currentHealthIndex;

        if (!isDead &&
            _currentHealthIndex < _health.Length &&
            _health[_currentHealthIndex] <= 0)
        {
            AdvancePhase();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (_currentHealthIndex >= _health.Length) return;

        _health[_currentHealthIndex] -= damage;

        PlaySound(takeDamageKeys, ref currentTakeDamageIndex);

        if (!isFlashing)
            StartCoroutine(DamageFlash());

        if (_health[_currentHealthIndex] <= 0)
            AdvancePhase();
    }

    private void AdvancePhase()
    {
        if (isDead) return;

        _currentHealthIndex++;

        if (_currentHealthIndex >= _health.Length)
        {
            isDead = true;
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer == null) yield break;

        isFlashing = true;
        spriteRenderer.color = flashColor;

        for (int i = 0; i < flashFrames; i++)
            yield return null;

        spriteRenderer.color = originalColor;
        isFlashing = false;
    }

    private void Die()
    {
        PlaySound(dieKeys, ref currentDieIndex);

        var scoreHandler = FindFirstObjectByType<ScoreHandler>();
        if (scoreHandler != null)
            scoreHandler.AddScore(scoreToAdd);

        if (dropItem != null)
            Instantiate(dropItem, transform.position, Quaternion.identity);

        if (particlePrefab != null)
            Instantiate(particlePrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            DamageCalculator damageCalculator = collision.GetComponent<DamageCalculator>();

            if (damageCalculator != null)
            {
                float damage = damageCalculator.CalculateDamage();
                TakeDamage(damage);
            }

            Destroy(collision.gameObject);
        }
    }

    private void PlaySound(string[] keys, ref int currentIndex)
    {
        if (keys != null && keys.Length > 0 && GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = keys[currentIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);
            currentIndex = (currentIndex + 1) % keys.Length;
        }
    }
}

