using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShotAttack
{
    public static void SimpleShot(Vector2 origin, Vector2 velocity)
    {
        Bullet bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;
    }

    public static void RadialShot(
        Vector2 center,
        Vector2 aimDirection,
        RadialShotSettings settings)
    {
        float angleBetweenBullets = 360f / settings.NumberOfBullets;

        if (settings.AngleOffset != 0f || settings.PhaseOffset != 0f)
        {
            aimDirection = aimDirection.Rotate(
                settings.AngleOffset + (settings.PhaseOffset * angleBetweenBullets)
            );
        }

        float halfMask = settings.MaskAngle * 0.5f;

        for (int i = 0; i < settings.NumberOfBullets; i++)
        {
            float bulletAngle = angleBetweenBullets * i;
            Vector2 direction = aimDirection.Rotate(bulletAngle).normalized;

            // --- RADIAL MASK ---
            if (settings.RadialMask && settings.MaskAngle < 360f)
            {
                float signedAngle = Vector2.SignedAngle(aimDirection, direction);

                if (Mathf.Abs(signedAngle) > halfMask)
                    continue;
            }

            Bullet bullet = BulletPool.Instance.RequestBullet();
            if (bullet == null)
                continue;

            bullet.transform.position = center;
            bullet.Velocity = direction * settings.BulletSpeed;
            bullet.ApplyVisual(settings);
        }
    }
}