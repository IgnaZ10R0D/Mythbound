using System.Collections;
using UnityEngine;

public class EnemyOrchestrator : MonoBehaviour
{
    public enum EnemyActionState
    {
        Idle,
        Attack,
        Move,
        AttackMove
    }

    public EnemyActionState CurrentState { get; private set; } = EnemyActionState.Idle;

    [Header("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private MovementController movementController;

    [Header("Waves")]
    [SerializeField] private EnemyWave[] waves;

    [Header("Scene Weapons")]
    [SerializeField] private WaveWeapon[] waveWeapons;

    private int currentWave = -1;
    private Coroutine movementRoutine;

    private float timeFactor = 1f;

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged += OnTimeWarpChanged;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged -= OnTimeWarpChanged;
    }

    private void OnTimeWarpChanged(float factor)
    {
        timeFactor = factor;
    }

    private void Start()
    {
        DisableAllWeapons();
        CurrentState = EnemyActionState.Idle;
    }

    private void Update()
    {
        if (enemy.HealthIndex != currentWave)
        {
            currentWave = enemy.HealthIndex;
            ApplyWave(currentWave);
        }
    }

    private void ApplyWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Length)
            return;

        if (movementRoutine != null)
            StopCoroutine(movementRoutine);

        var wave = waves[waveIndex];

        DisableAllWeapons();

        if (wave.shootInParallel)
        {
            ActivateCurrentWaveWeapons();
            CurrentState = EnemyActionState.Attack;
        }

        if (wave.movements != null && wave.movements.Length > 0)
            movementRoutine = StartCoroutine(PlayMovementsLoop(wave));
        else if (!wave.shootInParallel)
        {
            ActivateCurrentWaveWeapons();
            CurrentState = EnemyActionState.Attack;
        }
    }

    private IEnumerator PlayMovementsLoop(EnemyWave wave)
    {
        int i = 0;

        while (currentWave >= 0 && currentWave < waves.Length && waves[currentWave] == wave)
        {
            if (wave.movements[i] != null)
            {
                var parameters = wave.movementParams[i] ?? new MovementParams();

                if (!wave.shootInParallel)
                {
                    ActivateCurrentWaveWeapons();
                    CurrentState = EnemyActionState.Attack;
                }

                if (parameters.waitTime > 0f)
                {
                    float elapsed = 0f;

                    while (elapsed < parameters.waitTime)
                    {
                        elapsed += Time.deltaTime * timeFactor;
                        yield return null;
                    }
                }

                if (!wave.shootInParallel)
                {
                    DisableCurrentWaveWeapons();
                    CurrentState = EnemyActionState.Idle;
                }

                movementController.PlayMovement(wave.movements[i], parameters);

                CurrentState = wave.shootInParallel
                    ? EnemyActionState.AttackMove
                    : EnemyActionState.Move;

                yield return new WaitUntil(() => !movementController.IsBusy);

                CurrentState = EnemyActionState.Idle;
            }

            i++;

            if (i >= wave.movements.Length)
                i = 0;
        }
    }

    private void ActivateCurrentWaveWeapons()
    {
        if (waveWeapons == null)
            return;

        foreach (var ww in waveWeapons)
        {
            if (ww.weapon != null && ww.waveIndex == currentWave)
                ww.weapon.enabled = true;
        }
    }

    private void DisableAllWeapons()
    {
        if (waveWeapons == null)
            return;

        foreach (var ww in waveWeapons)
            if (ww.weapon != null)
                ww.weapon.enabled = false;
    }

    private void DisableCurrentWaveWeapons()
    {
        if (waveWeapons == null) return;

        foreach (var ww in waveWeapons)
        {
            if (ww.weapon != null && ww.waveIndex == currentWave)
                ww.weapon.enabled = false;
        }
    }
}