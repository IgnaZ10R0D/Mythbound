using UnityEngine;

[CreateAssetMenu(menuName = "Movement/SineMovement")]
public class SineMovement : MovementBehaviour
{
    public override MovementInstance CreateInstance(Transform owner, MovementParams parameters)
    {
        return new SineMovementInstance(
            owner,
            parameters.direction != Vector2.zero ? parameters.direction : Vector2.down,
            parameters.speed,
            parameters.amplitude,
            parameters.frequency,
            parameters.duration,
            parameters.smoothStop,
            parameters.stopCurve
        );
    }
}
