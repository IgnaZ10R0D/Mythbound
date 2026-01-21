using UnityEngine;

public class ZigZagBulletBehaviourInstance : BulletBehaviourInstance
{
    private readonly ZigZagBulletBehaviour config;

    private Vector2 baseDirection;
    private float time;

    public ZigZagBulletBehaviourInstance(ZigZagBulletBehaviour config)
    {
        this.config = config;
    }

    public override void Initialize(Bullet bullet)
    {
        base.Initialize(bullet);

        if (bullet.Velocity.sqrMagnitude > 0f)
            baseDirection = bullet.Velocity.normalized;
        else
            baseDirection = Vector2.up;

        time = 0f;
    }

    public override void Tick(float deltaTime)
    {
        time += deltaTime;

        Vector2 perpendicular = new Vector2(-baseDirection.y, baseDirection.x);

        float offset = Mathf.Sin(time * config.frequency) * config.amplitude;

        Vector2 newDirection = (baseDirection + perpendicular * offset).normalized;

        float speed = bullet.Velocity.magnitude;
        bullet.Velocity = newDirection * speed;
    }
}

