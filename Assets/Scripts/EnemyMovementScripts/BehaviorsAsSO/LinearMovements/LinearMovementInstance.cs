using UnityEngine;

public class LinearMovementInstance : MovementInstance
{
    private MovementParams _params;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    public LinearMovementInstance(
        Transform owner,
        MovementParams parameters)
        : base(owner)
    {
        _params = parameters;

        _startPosition = owner.position;
        _targetPosition = parameters.targetPosition;
    }

    public override void Tick(float deltaTime)
    {
        if (isFinished)
            return;

        base.Tick(deltaTime);

        Vector2 current = owner.position;
        Vector2 toTarget = _targetPosition - current;

        if (toTarget.sqrMagnitude < 0.001f)
        {
            owner.position = new Vector3(
                _targetPosition.x,
                _targetPosition.y,
                owner.position.z
            );

            isFinished = true;
            return;
        }

        float speedMultiplier = 1f;

        if (_params.smoothStop && _params.duration > 0f)
        {
            float t = Mathf.Clamp01(elapsedTime / _params.duration);

            if (_params.stopCurve != null)
                speedMultiplier = _params.stopCurve.Evaluate(t);
        }

        float finalSpeed = _params.speed * speedMultiplier;

        Vector2 direction = toTarget.normalized;
        Vector2 newPosition = current + direction * finalSpeed * deltaTime;

        owner.position = new Vector3(
            newPosition.x,
            newPosition.y,
            owner.position.z
        );

        // Si se definió duración, también puede cortar por tiempo
        if (_params.duration > 0f && elapsedTime >= _params.duration)
        {
            isFinished = true;
        }
    }
}