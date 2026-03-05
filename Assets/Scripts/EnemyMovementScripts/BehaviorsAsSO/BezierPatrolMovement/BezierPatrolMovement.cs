using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Bezier Patrol")]
public class BezierMovement : MovementBehaviour
{
    public override MovementInstance CreateInstance(Transform owner, MovementParams parameters)
    {
        return new BezierMovementInstance(owner, parameters);
    }
}
