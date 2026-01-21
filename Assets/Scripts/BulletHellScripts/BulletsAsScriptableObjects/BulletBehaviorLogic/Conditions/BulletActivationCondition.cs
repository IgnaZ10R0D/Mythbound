using UnityEngine;

public abstract class BulletActivationCondition : ScriptableObject
{
    public abstract BulletActivationConditionInstance CreateInstance();
}
