using UnityEngine;

public class CurvedPatrolMovement : MonoBehaviour, IMovement
{
    public int[] waveWhereMove = { 1, 2, 3 };
    EnemyMovementHandler IMovement.enemyMovementScript { get; set; }

    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float stopDuration = 1f;
    [SerializeField] private float controlPointOffset = 3f;

    [SerializeField] private float[] arrayX;
    [SerializeField] private float[] arrayY;

    private int currentPointIndex = 0;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private float t = 0f;

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();

        if (arrayX.Length > 0 && arrayY.Length > 0)
            transform.position = new Vector3(arrayX[0], arrayY[0], 0f);
    }

    public void Move(Transform enemyTransform, float currentSpeed)
    {
        if (enemy == null)
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
                t = 0f;
                SetNextCurve();
            }
        }
        else
        {
            t += patrolSpeed * currentSpeed * Time.deltaTime;
            t = Mathf.Clamp01(t);

            enemyTransform.position = CalculateQuadraticBezierPoint(t);

            if (t >= 1f)
                isStopped = true;
        }
    }

    private void SetNextCurve()
    {
        if (arrayX.Length == 0 || arrayY.Length == 0)
            return;

        currentPointIndex = (currentPointIndex + 1) % Mathf.Min(arrayX.Length, arrayY.Length);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t)
    {
        Vector3 startPoint = new Vector3(arrayX[currentPointIndex], arrayY[currentPointIndex], 0f);
        Vector3 endPoint = new Vector3(
            arrayX[(currentPointIndex + 1) % arrayX.Length],
            arrayY[(currentPointIndex + 1) % arrayY.Length],
            0f
        );

        Vector3 previousPoint = currentPointIndex > 0
            ? new Vector3(
                arrayX[(currentPointIndex - 1) % arrayX.Length],
                arrayY[(currentPointIndex - 1) % arrayY.Length],
                0f
            )
            : startPoint;

        Vector3 direction = (startPoint - previousPoint).normalized;
        Vector3 controlPoint = startPoint + direction * controlPointOffset;

        return Mathf.Pow(1 - t, 2) * startPoint
             + 2f * (1 - t) * t * controlPoint
             + Mathf.Pow(t, 2) * endPoint;
    }
}



