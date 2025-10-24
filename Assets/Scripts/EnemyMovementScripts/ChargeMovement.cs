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

    private EnemyAttackHandler enemyAttackScript;
    private Transform player;
    private Vector3 chargeTargetPosition;
    private Enemy enemyScript;
    
    void OnEnable()
    {
        enemyScript = GetComponent<Enemy>();
        enemyAttackScript = GetComponent<EnemyAttackHandler>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (System.Array.Exists(waveWhereMove, wave => wave == enemyScript.HealthIndex))
        {
            if (enemyAttackScript != null && player != null)
            {
                if (isStopped)
                {
                    stopTimer += Time.deltaTime;
                    if (stopTimer >= stopDuration)
                    {
                        stopTimer = 0f;
                        isStopped = false;
                        isCharging = true;
                        enemyAttackScript.canAttack = false;  
                        chargeTargetPosition = player.position;
                    }
                }
                else if (isCharging)
                {
                    // Multiplicamos la velocidad por currentSpeed
                    Vector3 chargeMovement = (chargeTargetPosition - enemyTransform.position).normalized * chargeSpeed * currentSpeed * Time.deltaTime;
                    enemyTransform.Translate(chargeMovement);

                    if (Vector3.Distance(enemyTransform.position, chargeTargetPosition) <= stoppingDistance)
                    {
                        isStopped = true;
                        isCharging = false;
                        enemyAttackScript.canAttack = true;  
                    }
                }
                else
                {
                    chargeTimer += Time.deltaTime;
                    if (chargeTimer >= chargeDelay)
                    {
                        chargeTimer = 0f;
                        isCharging = true;
                        enemyAttackScript.canAttack = false;
                        chargeTargetPosition = player.position;
                    }
                }
            }
        }
        else
        {
            enemyAttackScript.canAttack = true;
        }
    }
}








