using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletActivationConditionInstance
{
    public bool IsActive { get; protected set; }
    public abstract void Initialize (Bullet bullet);
    public abstract void Tick (float deltaTime);
}
