using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehaviourInstance
{
    protected Bullet bullet;

    public virtual void Initialize(Bullet bullet)
    {
        this.bullet = bullet;
    }

    public abstract void Tick(float deltaTime);
}
