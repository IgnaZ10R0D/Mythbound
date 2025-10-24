using UnityEngine;

public interface IMovement
{
    void Move(Transform enemyTransform, float currentSpeed);
    EnemyMovementHandler enemyMovementScript { get; set; }
}