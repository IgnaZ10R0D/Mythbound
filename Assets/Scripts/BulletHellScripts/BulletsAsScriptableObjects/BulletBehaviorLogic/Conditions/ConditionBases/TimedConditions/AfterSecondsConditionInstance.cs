using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterSecondsConditionInstance : BulletActivationConditionInstance
{
    private float _remainingTime;

    public AfterSecondsConditionInstance(float delay)
    {
        _remainingTime = delay;
    }

    public override void Initialize(Bullet bullet)
    {
        //Nothing else needed rn
    }

    public override void Tick(float deltaTime)
    {
        if (IsActive)
            return;
        _remainingTime -= deltaTime;
        if (_remainingTime <= 0)
            IsActive = true;
    }
}
