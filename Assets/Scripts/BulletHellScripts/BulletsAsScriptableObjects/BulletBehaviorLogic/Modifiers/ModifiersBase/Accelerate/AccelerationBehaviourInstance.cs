using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationBehaviourInstance : BulletBehaviourInstance
{
    private float _acceleration;

    public AccelerationBehaviourInstance(float acceleration)
    {
        _acceleration = acceleration;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 direction = bullet.Velocity.normalized;
        bullet.Velocity += direction * _acceleration * deltaTime;
    }
}

