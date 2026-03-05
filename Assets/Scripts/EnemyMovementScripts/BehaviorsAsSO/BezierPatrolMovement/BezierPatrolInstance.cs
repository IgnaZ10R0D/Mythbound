using UnityEngine;

public class BezierMovementInstance : MovementInstance
{
    private MovementParams _params;

    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private Vector2 _controlPoint;

    private float _t;
    private Vector2 _velocity;

    private float _steeringStrength = 15f;
    private float _drag = 3f;
    private float _lookAhead = 0.1f;

    public BezierMovementInstance(
        Transform owner,
        MovementParams parameters)
        : base(owner)
    {
        _params = parameters;

        _startPoint = owner.position;
        _endPoint = parameters.targetPosition;

        Vector2 direction = (_endPoint - _startPoint).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        _controlPoint =
            Vector2.Lerp(_startPoint, _endPoint, 0.5f)
            + perpendicular * _params.controlPointOffset;

        _t = 0f;
        _velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        if (isFinished)
            return;

        base.Tick(deltaTime);

        float curveLengthApprox = Vector2.Distance(_startPoint, _endPoint);
        float deltaT = (_params.speed / curveLengthApprox) * deltaTime;

        _t += deltaT;
        _t = Mathf.Clamp01(_t);

        float targetT = Mathf.Clamp01(_t + _lookAhead);
        Vector2 targetPoint = CalculateQuadraticBezier(targetT);

        Vector2 current = owner.position;

        Vector2 desiredVelocity =
            (targetPoint - current).normalized * _params.speed;

        Vector2 steering =
            (desiredVelocity - _velocity) * _steeringStrength;

        _velocity += steering * deltaTime;

        _velocity *= Mathf.Clamp01(1f - _drag * deltaTime);

        owner.position += (Vector3)(_velocity * deltaTime);

        if (_t >= 1f && Vector2.Distance(owner.position, _endPoint) < 0.2f)
        {
            owner.position = _endPoint;
            isFinished = true;
        }
    }

    private Vector2 CalculateQuadraticBezier(float t)
    {
        float u = 1f - t;

        return u * u * _startPoint
             + 2f * u * t * _controlPoint
             + t * t * _endPoint;
    }
}
