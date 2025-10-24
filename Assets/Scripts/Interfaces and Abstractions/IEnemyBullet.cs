using UnityEngine;

public interface IEnemyBullet
{
    
    float Duration { get; set; }
    float MovementSpeed { get; set; }

    void ApplyConfig(BulletConfig config);
    void BulletMovement();
}

