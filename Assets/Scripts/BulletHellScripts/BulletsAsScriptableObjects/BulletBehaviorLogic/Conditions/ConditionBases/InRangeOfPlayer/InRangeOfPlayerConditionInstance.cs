using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeOfPlayerConditionInstance : BulletActivationConditionInstance
{
    private float _range;
    private Bullet _bullet;
    private Transform _playerTransform;

    public InRangeOfPlayerConditionInstance(float range)
    {
        _range = range;
    }

    public override void Initialize(Bullet bullet)
    {
        _bullet = bullet;
        Player player = Object.FindObjectOfType<Player>();
        if (player != null)
            _playerTransform = player.transform;
    }

    public override void Tick(float deltaTime)
    {
        if (IsActive || _playerTransform == null)
            return;
        float distance = Vector2.Distance(_bullet.transform.position, _playerTransform.position);
        if (distance <= _range)
            IsActive = true;
    }
}
