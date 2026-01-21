using UnityEngine;

public class ChargeMovement : MonoBehaviour, IMovement
{
    public int[] waveWhereMove = { 1, 2, 3 };
    EnemyMovementHandler IMovement.enemyMovementScript { get; set; }

    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDelay = 2f;
    [SerializeField] private float stopDuration = 3f;
    [SerializeField] private float stoppingDistance = 0.5f;

    private float chargeTimer = 0f;
    private float stopTimer = 0f;
    private bool isCharging = false;
    private bool isStopped = false;

    private Transform player;
    private Vector3 chargeTargetPosition;
    private Enemy enemy;

    private void OnEnable()
    {
        enemy = GetComponent<Enemy>();
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (enemy == null || player == null)
            return;

        if (!System.Array.Exists(waveWhereMove, wave => wave == enemy.HealthIndex))
            return;

        if (isStopped)
        {
            stopTimer += Time.deltaTime * currentSpeed;

            if (stopTimer >= stopDuration)
            {
                stopTimer = 0f;
                isStopped = false;
                isCharging = true;
                chargeTargetPosition = player.position;
            }
        }
        else if (isCharging)
        {
            Vector3 direction = (chargeTargetPosition - enemyTransform.position).normalized;
            Vector3 movement = direction * chargeSpeed * currentSpeed * Time.deltaTime;

            enemyTransform.Translate(movement);

            if (Vector3.Distance(enemyTransform.position, chargeTargetPosition) <= stoppingDistance)
            {
                isStopped = true;
                isCharging = false;
            }
        }
        else
        {
            chargeTimer += Time.deltaTime * currentSpeed;

            if (chargeTimer >= chargeDelay)
            {
                chargeTimer = 0f;
                isCharging = true;
                chargeTargetPosition = player.position;
            }
        }
    }
}


