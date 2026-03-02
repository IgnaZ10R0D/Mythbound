using UnityEngine;

public class SineMovementInstance : MovementInstance
{
    private Vector2 direction;
    private float speed;
    private float amplitude;
    private float frequency;

    private bool smoothStop;
    private AnimationCurve stopCurve;

    private Vector2 startPosition;

    public SineMovementInstance(
        Transform owner,
        Vector2 direction,
        float speed,
        float amplitude,
        float frequency,
        float duration,
        bool smoothStop,
        AnimationCurve stopCurve)
        : base(owner, duration)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.smoothStop = smoothStop;
        this.stopCurve = stopCurve;
    }

    public override void Start()
    {
        base.Start();
        startPosition = owner.position;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (isFinished)
            return;

        float speedMultiplier = 1f;

        if (smoothStop && duration > 0f)
        {
            float t = GetNormalizedTime();
            speedMultiplier = stopCurve.Evaluate(t);
        }

        float forward = speed * speedMultiplier * elapsedTime;
        float offset = Mathf.Sin(elapsedTime * frequency) * amplitude;

        Vector2 move = direction * forward;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * offset;

        Vector2 newPos = startPosition + move + perpendicular;

        owner.position = new Vector3(
            newPos.x,
            newPos.y,
            owner.position.z
        );
    }
}