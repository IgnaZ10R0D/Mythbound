public interface IAttack 
{
    void Attack();
    int[] HealthIndexes { get; }
    EnemyAttackHandler enemyAttackScript { get; set; }
    bool IsContinuous { get; } 
}
