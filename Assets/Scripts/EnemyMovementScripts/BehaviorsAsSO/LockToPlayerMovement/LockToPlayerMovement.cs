using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Movement/LockToPlayerMovement")]
public class LockToPlayerMovement : MovementBehaviour
{
    public float arrivalThreshold = 0.1f;

    public override MovementInstance CreateInstance(Transform owner, MovementParams parameters)
    {
        return new LockToPlayerMovementInstance(this, owner, parameters);
    }
}
