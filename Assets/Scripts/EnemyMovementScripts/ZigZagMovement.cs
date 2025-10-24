using UnityEngine;

public class ZigzagMovement : MonoBehaviour, IMovement
{
    [SerializeField] private float initialX;
    [SerializeField] private float targetX;
    private bool movingToTarget = true;

    public EnemyMovementHandler enemyMovementScript { get; set; }
    public Enemy enemy;
    public int[] waveWhereMove { get; set; }

    void Start()
    {
        Vector3 startPosition = transform.position;
        startPosition.x = initialX;
        transform.position = startPosition;
    }

    // Move actualizado para usar currentSpeed
    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (System.Array.Exists(waveWhereMove, wave => wave == enemy.HealthIndex)) 
        { 
            if (enemyMovementScript == null) return;

            // Movimiento vertical
            Vector3 verticalMovement = Vector3.down * currentSpeed * Time.deltaTime;

            float currentX = enemyTransform.position.x;
            float newX;

            // Movimiento horizontal zigzag
            if (movingToTarget)
            {
                newX = Mathf.MoveTowards(currentX, targetX, currentSpeed * Time.deltaTime);
                if (Mathf.Abs(newX - targetX) < 0.01f)
                {
                    movingToTarget = false; 
                }
            }
            else
            {
                newX = Mathf.MoveTowards(currentX, initialX, currentSpeed * Time.deltaTime);
                if (Mathf.Abs(newX - initialX) < 0.01f)
                {
                    movingToTarget = true; 
                }
            }

            Vector3 movement = new Vector3(newX - currentX, verticalMovement.y, 0f);
            enemyTransform.Translate(movement, Space.World);
        }
    }
}


