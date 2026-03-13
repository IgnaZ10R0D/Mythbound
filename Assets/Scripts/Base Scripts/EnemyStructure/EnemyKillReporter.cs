using UnityEngine;

public class EnemyKillReporter : MonoBehaviour
{
    private LevelController _levelController;
    private Enemy _enemy;

    private bool _reported = false;

    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnDestroy()
    {
        if (_reported)
            return;

        if (_enemy == null)
            return;
        if (_enemy.CurrentHealth <= 0)
        {
            if (_levelController != null)
            {
                _levelController.RegisterKill();
                _reported = true;
            }
        }
    }
}
