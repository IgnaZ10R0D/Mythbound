using System;
using UnityEngine;

public sealed class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private float secondsBetweenShots = 0.5f;
    public bool canAttack = false;

    private float attackTimer = 0f;
    private Renderer enemyRenderer;
    private bool isAttacking = false;
    private EnemyAnimationController animationController;
    private IAttack[] cachedAttacks;
    private float timeFactor = 1f; 

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        animationController = GetComponent<EnemyAnimationController>();
        cachedAttacks = GetComponents<IAttack>();

        if (TimeManager.Instance != null)
        {
            timeFactor = TimeManager.Instance.TimeSlow;
            TimeManager.Instance.OnTimeWarpChanged += UpdateTimeFactor;
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeWarpChanged -= UpdateTimeFactor;
        }
    }

    private void UpdateTimeFactor(float factor)
    {
        timeFactor = factor;
    }

    void Update()
    {
        if (!canAttack || Time.timeScale == 0 || !IsInCameraBounds()) return;

        int currentHealthIndex = GetComponent<Enemy>().HealthIndex;

        foreach (IAttack attack in cachedAttacks)
        {
            if (!Array.Exists(attack.HealthIndexes, index => index == currentHealthIndex))
                continue;

            if (attack.IsContinuous)
            {
                attack.Attack();
            }
        }

        if (!isAttacking)
        {
            attackTimer += Time.deltaTime * timeFactor; 

            if (attackTimer >= secondsBetweenShots)
            {
                StartAttack();
                attackTimer = 0f;
            }
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        animationController?.SetAttacking(true);

        int currentHealthIndex = GetComponent<Enemy>().HealthIndex;

        foreach (IAttack attack in cachedAttacks)
        {
            if (!attack.IsContinuous && Array.Exists(attack.HealthIndexes, index => index == currentHealthIndex))
            {
                attack.Attack();
            }
        }

        float stopDelay = 0.5f / Mathf.Max(timeFactor, 0.01f); 
        Invoke(nameof(StopAttack), stopDelay);
    }

    private void StopAttack()
    {
        isAttacking = false;
        animationController?.SetAttacking(false);
    }

    private bool IsInCameraBounds()
    {
        return enemyRenderer != null && enemyRenderer.isVisible;
    }
}



