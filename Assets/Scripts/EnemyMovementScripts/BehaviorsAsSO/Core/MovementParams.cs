using UnityEngine;

[System.Serializable]
public class MovementParams
{
    public float waitTime;

    public MovementTargetType targetType = MovementTargetType.FixedPosition;

    public Vector2 targetPosition;

    public Vector2 targetOffset;

    public float speed;
    public float duration;

    public bool smoothStop = false;
    public AnimationCurve stopCurve = AnimationCurve.Linear(0, 1, 1, 0);

    public float controlPointOffset = 3f;
}
public enum MovementTargetType
{
    FixedPosition,
    Player
}
