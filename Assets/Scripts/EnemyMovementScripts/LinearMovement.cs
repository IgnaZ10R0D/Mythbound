using UnityEngine;

public class LinearMovement : MonoBehaviour, IMovement
{
    [SerializeField] private int[] waveWhereMove = { 1, 2, 3 };  
    public EnemyMovementHandler enemyMovementScript { get; set; }
    public Enemy enemyScript;

    void Start()
    {
        enemyMovementScript = GetComponent<EnemyMovementHandler>();
        enemyScript = GetComponent<Enemy>();
    }

    // Move actualizado para usar currentSpeed
    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (System.Array.Exists(waveWhereMove, wave => wave == enemyScript.HealthIndex))
        {
            enemyTransform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
        }
    }
}







