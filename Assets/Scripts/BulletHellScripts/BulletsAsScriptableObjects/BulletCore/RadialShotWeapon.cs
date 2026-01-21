using System.Collections;
using UnityEngine;

public class RadialShotWeapon : MonoBehaviour
{
    [SerializeField] private RadialShotPattern _shotPattern;
    private bool _onShotPattern = false;

    private void Update()
    {
        if (_onShotPattern)
            return;

        // Do not initiate pattern until it is inside CameraBounds.
        if (!IsInsideCameraBounds())
            return;

        StartCoroutine(ExecuteRadialShotPattern(_shotPattern));
    }

    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
    {
        _onShotPattern = true;

        int lap = 0;
        Vector2 aimDirection = Vector2.up;

        yield return new WaitForSeconds(pattern.StartWait);

        while (lap < pattern.Repetitions)
        {
            // If it isn't inside camera bounds, it doesn't shoot 
            if (!IsInsideCameraBounds())
                break;

            if (lap > 0 && pattern.AngleOffsetBetweenReps != 0f)
                aimDirection = aimDirection.Rotate(pattern.AngleOffsetBetweenReps);

            for (int i = 0; i < pattern.PatternSettings.Length; i++)
            {
                // Additional check just in case.
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
    }

    private bool IsInsideCameraBounds()
    {
        if (Camera.main == null)
            return true;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.z <= 0f)
            return false;

        if (viewportPos.x < 0f || viewportPos.x > 1f)
            return false;

        if (viewportPos.y < 0f || viewportPos.y > 1f)
            return false;

        return true;
    }
}


