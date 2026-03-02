using UnityEngine;

public class LockToPlayerMovementInstance : MovementInstance
{
    private LockToPlayerMovement _settings;
    private MovementParams _parameters;

    private Vector2 _targetPosition;
    private float _initialDistance;

    private static Player _cachedPlayer;

    public LockToPlayerMovementInstance(
        LockToPlayerMovement settings,
        Transform owner,
        MovementParams parameters)
        : base(owner)
    {
        _settings = settings;
        _parameters = parameters;

        CachePlayer();
        LockTarget();

        _initialDistance = Vector2.Distance(owner.position, _targetPosition);
    }

    private void CachePlayer()
    {
        if (_cachedPlayer == null)
            _cachedPlayer = Object.FindObjectOfType<Player>();
    }

    private void LockTarget()
    {
        if (_cachedPlayer != null)
            _targetPosition = (Vector2)_cachedPlayer.transform.position + _parameters.offset;
        else
            _targetPosition = owner.position;
    }

    public override void Tick(float deltaTime)
    {
        if (isFinished)
            return;

        Vector2 current = owner.position;
        float remainingDistance = Vector2.Distance(current, _targetPosition);

        if (remainingDistance <= _settings.arrivalThreshold)
        {
            owner.position = _targetPosition;
            isFinished = true;
            return;
        }

        float speed = _parameters.speed;

        if (_parameters.smoothStop && _initialDistance > 0f)
        {
            float normalized = remainingDistance / _initialDistance;
            speed *= _parameters.stopCurve.Evaluate(1f - normalized);
        }

        Vector2 newPos = Vector2.MoveTowards(
            current,
            _targetPosition,
            speed * deltaTime
        );

        owner.position = newPos;
    }
}