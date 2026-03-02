using UnityEngine;

public class LinearMovementInstance : MovementInstance
{
    private MovementParams _params;
    private Vector2 _direction;

    public LinearMovementInstance(
        Transform owner,
        MovementParams parameters)
        : base(owner)
    {
        _params = parameters;
        _direction = parameters.direction.normalized;
    }

    public override void Tick(float deltaTime)
    {
        if (isFinished)
            return;

        base.Tick(deltaTime);

        if (_params.duration <= 0f)
        {
            isFinished = true;
            return;
        }

        float speedMultiplier = 1f;

        if (_params.smoothStop)
        {
            float t = Mathf.Clamp01(elapsedTime / _params.duration);

            if (_params.stopCurve != null)
                speedMultiplier = _params.stopCurve.Evaluate(t);
        }

        float finalSpeed = _params.speed * speedMultiplier;

        Vector2 current = owner.position;
        current += _direction * finalSpeed * deltaTime;

        owner.position = new Vector3(
            current.x,
            current.y,
            owner.position.z
        );

        if (elapsedTime >= _params.duration)
        {
            isFinished = true;
        }
    }
}
