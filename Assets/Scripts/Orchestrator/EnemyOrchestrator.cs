using System.Collections;
using UnityEngine;

public class EnemyOrchestrator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private MovementController movementController;

    [Header("Waves")]
    [SerializeField] private EnemyWave[] waves;

    [Header("Scene Weapons")]
    [SerializeField] private WaveWeapon[] waveWeapons;

    private int currentWave = -1;

    private void Start()
    {
        DisableAllWeapons();
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

        var wave = waves[waveIndex];

        DisableAllWeapons();

        if (wave.shootInParallel)
            ActivateCurrentWaveWeapons();

        if (wave.movements != null && wave.movements.Length > 0)
            StartCoroutine(PlayMovementsSequentially(wave));
        else if (!wave.shootInParallel)
            ActivateCurrentWaveWeapons(); 
    }

    private IEnumerator PlayMovementsSequentially(EnemyWave wave)
    {
        for (int i = 0; i < wave.movements.Length; i++)
        {
            if (wave.movements[i] == null)
            {
                continue;
            }

            var parameters = wave.movementParams[i] ?? new MovementParams();

            if (!wave.shootInParallel)
            {
                ActivateCurrentWaveWeapons();
            }

            if (parameters.waitTime > 0f)
            {
                float elapsed = 0f;
                while (elapsed < parameters.waitTime)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

            if (!wave.shootInParallel)
            {
                DisableCurrentWaveWeapons();
            }

            movementController.PlayMovement(wave.movements[i], parameters);

            yield return new WaitUntil(() => !movementController.IsBusy);
        }

        if (!wave.shootInParallel)
        {
            ActivateCurrentWaveWeapons();
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
            {
                ww.weapon.enabled = false;
            }
        }
    }
}