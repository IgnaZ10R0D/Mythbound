using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Behaviours/ConstantVelocity")]
public class ConstantVelocityBehaviour : BulletBehaviour
{
    public override BulletBehaviourInstance CreateInstance()
    {
        return new ConstantVelocityBehaviourInstance();
    }
}
