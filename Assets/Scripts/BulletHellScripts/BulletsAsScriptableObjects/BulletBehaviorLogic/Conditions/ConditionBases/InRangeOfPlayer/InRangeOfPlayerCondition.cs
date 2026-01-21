using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Activation Conditions/In Range of Player")]
public class InRangeOfPlayerCondition : BulletActivationCondition
{
    public float Range;

    public override BulletActivationConditionInstance CreateInstance()
    {
        return new InRangeOfPlayerConditionInstance(Range);
    }
}
