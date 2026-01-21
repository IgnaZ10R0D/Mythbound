using UnityEngine;

public sealed class EnemyAttackHandler : MonoBehaviour
{
    [Header("Attack Entries")]
    [SerializeField] private AttackEntry[] attackEntries;

    private Renderer enemyRenderer;
    private Enemy enemy;

    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        UpdateAttackObjects();
    }

    private void Update()
    {
        if (Time.timeScale == 0 || !IsInCameraBounds())
            return;

        UpdateAttackObjects();
    }

    private void UpdateAttackObjects()
    {
        if (attackEntries == null || enemy == null)
            return;

        int currentIndex = enemy.HealthIndex;

        foreach (var entry in attackEntries)
        {
            if (entry.AttackObject == null)
                continue;

            bool shouldBeActive = entry.HealthIndex == currentIndex;

            if (entry.AttackObject.activeSelf != shouldBeActive)
                entry.AttackObject.SetActive(shouldBeActive);
        }
    }

    private bool IsInCameraBounds()
    {
        return enemyRenderer != null && enemyRenderer.isVisible;
    }

    [System.Serializable]
    public class AttackEntry
    {
        public GameObject AttackObject;
        public int HealthIndex;
    }
}


