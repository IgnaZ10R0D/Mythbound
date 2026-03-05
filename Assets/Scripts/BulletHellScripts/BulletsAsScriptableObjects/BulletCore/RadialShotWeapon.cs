using System.Collections;
using UnityEngine;

public class RadialShotWeapon : MonoBehaviour
{
    [SerializeField] private RadialShotPattern _shotPattern;

    private bool _onShotPattern = false;
    private Coroutine currentPattern;

    private void OnEnable()
    {
        ResetWeaponState();
    }

    private void OnDisable()
    {
        StopCurrentPattern();
    }

    private void ResetWeaponState()
    {
        StopCurrentPattern();
        _onShotPattern = false;
    }

    private void StopCurrentPattern()
    {
        if (currentPattern != null)
        {
            StopCoroutine(currentPattern);
            currentPattern = null;
        }
    }

    private void Update()
    {
        if (_onShotPattern)
            return;

        if (!IsInsideCameraBounds())
            return;

        currentPattern = StartCoroutine(ExecuteRadialShotPattern(_shotPattern));
    }

    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
    {
        _onShotPattern = true;

        int lap = 0;
        Vector2 aimDirection = Vector2.up;

        yield return new WaitForSeconds(pattern.StartWait);

        while (lap < pattern.Repetitions)
        {
            if (!isActiveAndEnabled)
                yield break;

            if (!IsInsideCameraBounds())
                break;

            if (lap > 0 && pattern.AngleOffsetBetweenReps != 0f)
                aimDirection = aimDirection.Rotate(pattern.AngleOffsetBetweenReps);

            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                if (!isActiveAndEnabled)
                    yield break;

                if (!IsInsideCameraBounds())
                    break;

                Vector2 center = transform.position;

                ShotAttack.RadialShot(
                    center,
                    aimDirection,
                    pattern.PatternSettings[i]
                );

                yield return new WaitForSeconds(
                    pattern.PatternSettings[i].CooldownAfterShot
                );
            }

            lap++;
        }

        yield return new WaitForSeconds(pattern.EndWait);

        _onShotPattern = false;
        currentPattern = null;
    }

    private bool IsInsideCameraBounds()
    {
        if (Camera.main == null)
            return true;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.z <= 0f)
            return false;

        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }
}

