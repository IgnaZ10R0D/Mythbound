using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float[] _health = { 100f };
    public int scoreToAdd = 100;
    private int _currentHealthIndex = 0;
    public int HealthIndex;

    [SerializeField] private GameObject dropItem;
    [SerializeField] private ParticleSystem particlePrefab;

    [Header("Animation Profiles")]
    [SerializeField] private EnemyAnimationProfile[] animationProfiles;
    private EnemyAnimationController animationController;

    public float CurrentHealth => _health[_currentHealthIndex];
    public float MaxHealth => _health.Length > 0 ? _health[_currentHealthIndex] : 0f;

    void Start()
    {
        animationController = GetComponent<EnemyAnimationController>();
        if (animationController != null && animationProfiles.Length > 0)
        {
            animationController.SetAnimationProfile(animationProfiles[0]);
        }

        var scoreHandler = FindAnyObjectByType<ScoreHandler>();
    }

    void Update()
    {
        if (HealthIndex != _currentHealthIndex)
        {
            HealthIndex = _currentHealthIndex;

            if (animationController != null && animationProfiles.Length > HealthIndex)
            {
                animationController.SetAnimationProfile(animationProfiles[HealthIndex]);
            }
        }

        CheckBoundaries();

        if (_health[_currentHealthIndex] <= 0)
        {
            _currentHealthIndex++;
            if (_currentHealthIndex >= _health.Length)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        EnemySounds enemySounds = GetComponent<EnemySounds>();
        if (_currentHealthIndex < _health.Length)
        {
            _health[_currentHealthIndex] -= damage;
            if (enemySounds != null)
            {
                enemySounds.PlaySound("TakeDamage");
            }
            if (_health[_currentHealthIndex] <= 0)
            {
                _currentHealthIndex++;
                if (_currentHealthIndex >= _health.Length)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        bool allHealthEmpty = true;
        foreach (float healthValue in _health)
        {
            if (healthValue > 0f)
            {
                allHealthEmpty = false;
                break;
            }
        }

        if (allHealthEmpty)
        {
            var scoreHandler = FindFirstObjectByType<ScoreHandler>();
            if (scoreHandler != null)
            {
                scoreHandler.AddScore(scoreToAdd);
            }
            if (dropItem != null)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
            if (particlePrefab != null)
            {
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
        }
        ;

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

    private void CheckBoundaries()
    {
        if (transform.position.x < EnemyMovementHandler.minX || transform.position.x > EnemyMovementHandler.maxX ||
            transform.position.y < EnemyMovementHandler.minY || transform.position.y > EnemyMovementHandler.maxY)
        {
            Destroy(gameObject);
        }
    }
}


