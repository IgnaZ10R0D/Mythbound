using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Behaviours/Homing")]
public class HomingBehaviour : BulletBehaviour
{
    public float TurnSpeed;
    public override BulletBehaviourInstance CreateInstance()
    {
        return new HomingBehaviourInstance(TurnSpeed);
    }
}
