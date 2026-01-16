using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Bullets/Activation Conditions/On Spawn")]
public class OnSpawnCondition : BulletActivationCondition
{
    public override BulletActivationConditionInstance CreateInstance()
    {
        return new OnSpawnConditionInstance();
    }
}
