using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehaviourInstance : BulletBehaviourInstance
{
    private float _turnSpeed;
    private Transform _player;

    public HomingBehaviourInstance(float turnSpeed)
    {
        _turnSpeed = turnSpeed;
    }

    public override void Initialize(Bullet bullet)
    {
        base.Initialize(bullet);
        Player player =  Object.FindObjectOfType<Player>();
        if (player != null)
            _player = player.transform;
    }

    public override void Tick(float deltaTime)
    {
        if (_player == null)
            return;
        Vector2 toPlayer = ((Vector2)_player.position - (Vector2)bullet.transform.position).normalized;
        Vector2 currentDir = bullet.Velocity.normalized;
        
        Vector2 newDir = Vector2.Lerp(currentDir, toPlayer, _turnSpeed * deltaTime).normalized;
        bullet.Velocity = newDir * bullet.Velocity.magnitude;
    }
}
