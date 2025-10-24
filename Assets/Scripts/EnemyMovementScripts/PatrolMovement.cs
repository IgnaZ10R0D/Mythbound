using UnityEngine;

public class PatrolMovement : MonoBehaviour, IMovement
{
    public int[] waveWhereMove = { 1, 2, 3 };
    EnemyMovementHandler IMovement.enemyMovementScript { get; set; }

    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float stopDuration = 3f;
    [SerializeField] private float stoppingDistance = 0.5f;

    [SerializeField] private float[] arrayX;
    [SerializeField] private float[] arrayY;
    private int currentTargetIndex = 0;

    private float stopTimer = 0f;
    private bool isStopped = false;

    private EnemyAttackHandler enemyScript;
    private Enemy enemy;
    private Vector3 patrolTargetPosition;

    void Start()
    {
        enemy = GetComponent<Enemy>();        
        enemyScript = GetComponent<EnemyAttackHandler>();       
        SetNextPatrolTarget();
    }

    // Move actualizado para usar currentSpeed
    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (enemy == null || enemyScript == null) return;

        if (System.Array.Exists(waveWhereMove, wave => wave == enemy.HealthIndex))
        {
            if (isStopped)
            {
                stopTimer += Time.deltaTime * currentSpeed;
                if (stopTimer >= stopDuration)
                {
                    stopTimer = 0f;
                    isStopped = false;
                    enemyScript.canAttack = false;
                    SetNextPatrolTarget();
                }
            }
            else
            {
                Vector3 patrolMovement = (patrolTargetPosition - enemyTransform.position).normalized * patrolSpeed * currentSpeed * Time.deltaTime;
                enemyTransform.Translate(patrolMovement);

                if (Vector3.Distance(enemyTransform.position, patrolTargetPosition) <= stoppingDistance)
                {
                    isStopped = true;
                    enemyScript.canAttack = true;
                }
            }
        }
        else
        {
            enemyScript.canAttack = true;
        }
    }

    private void SetNextPatrolTarget()
    {
        if (currentTargetIndex < arrayX.Length || currentTargetIndex < arrayY.Length)
        {
            float xPosition = currentTargetIndex < arrayX.Length ? arrayX[currentTargetIndex] : arrayX[arrayX.Length - 1];
            float yPosition = currentTargetIndex < arrayY.Length ? arrayY[currentTargetIndex] : arrayY[arrayY.Length - 1];

            patrolTargetPosition = new Vector3(xPosition, yPosition, 0f);
            currentTargetIndex++;
        }
        else
        {
            currentTargetIndex = 0;
            SetNextPatrolTarget();
        }
    }
}


