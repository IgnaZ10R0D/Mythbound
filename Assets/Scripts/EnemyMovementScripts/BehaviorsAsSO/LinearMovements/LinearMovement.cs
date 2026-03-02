using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Linear Movement")]
public class LinearMovement : MovementBehaviour
{
    public override MovementInstance CreateInstance(Transform owner, MovementParams parameters)
    {
        return new LinearMovementInstance(owner, parameters);
    }
}
