using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bullets/Behaviours/Acceleration")]
public class AccelerationBehaviour : BulletBehaviour
{
    public float Acceleration;
    public override BulletBehaviourInstance CreateInstance()
    {
        return new AccelerationBehaviourInstance(Acceleration);
    }
}
