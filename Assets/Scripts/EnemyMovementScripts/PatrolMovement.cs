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

    private Enemy enemy;
    private Vector3 patrolTargetPosition;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        SetNextPatrolTarget();
    }

    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (enemy == null)
            return;

        if (System.Array.Exists(waveWhereMove, wave => wave == enemy.HealthIndex))
        {
            if (isStopped)
            {
                stopTimer += Time.deltaTime * currentSpeed;

                if (stopTimer >= stopDuration)
                {
                    stopTimer = 0f;
                    isStopped = false;
                    SetNextPatrolTarget();
                }
            }
            else
            {
                Vector3 direction = (patrolTargetPosition - enemyTransform.position).normalized;
                Vector3 movement = direction * patrolSpeed * currentSpeed * Time.deltaTime;

                enemyTransform.Translate(movement);

                if (Vector3.Distance(enemyTransform.position, patrolTargetPosition) <= stoppingDistance)
                {
                    isStopped = true;
                }
            }
        }
    }

    private void SetNextPatrolTarget()
    {
        if (arrayX.Length == 0 || arrayY.Length == 0)
            return;

        float x = currentTargetIndex < arrayX.Length
            ? arrayX[currentTargetIndex]
            : arrayX[arrayX.Length - 1];

        float y = currentTargetIndex < arrayY.Length
            ? arrayY[currentTargetIndex]
            : arrayY[arrayY.Length - 1];

        patrolTargetPosition = new Vector3(x, y, 0f);

        currentTargetIndex++;

        if (currentTargetIndex >= Mathf.Max(arrayX.Length, arrayY.Length))
            currentTargetIndex = 0;
    }
}



