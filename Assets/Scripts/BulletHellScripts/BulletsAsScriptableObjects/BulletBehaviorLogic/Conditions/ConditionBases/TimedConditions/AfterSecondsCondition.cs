using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Bullets/Activation Conditions/After Seconds")]
public class AfterSecondsCondition : BulletActivationCondition
{
    public float Delay;

    public override BulletActivationConditionInstance CreateInstance()
    {
        return new AfterSecondsConditionInstance(Delay);
    }
}
