using UnityEngine;
using System.Collections.Generic;

public class BridgeWrathAttack : MonoBehaviour, IAttack
{
    [System.Serializable]
    public class BulletEntry
    {
        public GameObject prefab;
        public BulletConfig config;
    }

    [Header("Bridge Arcs")]
    [SerializeField] private List<BulletEntry> arcBulletVariants = new List<BulletEntry>();
    [SerializeField] private int bulletsPerArc = 12;
    [SerializeField] private float arcRadius = 4f;

    [Header("Vertical Pillars")]
    [SerializeField] private GameObject pillarBulletPrefab;
    [SerializeField] private BulletConfig pillarConfig;
    [SerializeField] private int pillarCount = 3;
    [SerializeField] private float pillarSpacing = 2f;
    [SerializeField] private float pillarYOffset = 10f;

    [Header("Timing")]
    [SerializeField] private float timeBetweenArcs = 1.5f;
    [SerializeField] private float timeBetweenPillars = 4f;

    [Header("Audio")]
    [SerializeField] private string[] attackSoundKeys;
    private int soundIndex;

    [Header("Health Phases")]
    [SerializeField] private int[] healthIndexes = { 4 }; 
    public int[] HealthIndexes => healthIndexes;

    public bool IsContinuous => true; 
    public EnemyAttackHandler enemyAttackScript { get; set; }

    private float arcTimer;
    private float pillarTimer;
    private int arcVariantIndex;

    private void Start()
    {
        enemyAttackScript = GetComponent<EnemyAttackHandler>();
        arcTimer = 0f;
        pillarTimer = 0f;
    }

    public void Attack()
    {
        arcTimer -= Time.deltaTime;
        pillarTimer -= Time.deltaTime;

        if (arcTimer <= 0f)
        {
            SpawnArcOfBullets();
            arcTimer = timeBetweenArcs;
        }

        if (pillarTimer <= 0f)
        {
            SpawnVerticalPillars();
            pillarTimer = timeBetweenPillars;
        }
    }

    private void SpawnArcOfBullets()
    {
        if (arcBulletVariants.Count == 0) return;

        float angleStep = 180f / (bulletsPerArc - 1); // arco semicircular
        BulletEntry entry = arcBulletVariants[arcVariantIndex];

        for (int i = 0; i < bulletsPerArc; i++)
        {
            float angle = -90f + i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(angleRad) * arcRadius,
                Mathf.Sin(angleRad) * arcRadius,
                0f
            );

            Vector3 spawnPosition = transform.position + offset;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject bullet = Instantiate(entry.prefab, spawnPosition, rotation);
            ApplyConfigToBullet(bullet, entry.config);
        }

        arcVariantIndex = (arcVariantIndex + 1) % arcBulletVariants.Count;

        PlayAttackSound();
    }

    private void SpawnVerticalPillars()
    {
        if (pillarBulletPrefab == null || pillarConfig == null) return;

        float startX = transform.position.x - (pillarSpacing * (pillarCount - 1) / 2f);

        for (int i = 0; i < pillarCount; i++)
        {
            float xPos = startX + i * pillarSpacing;
            Vector3 spawnPos = new Vector3(xPos, transform.position.y + pillarYOffset, 0f);

            GameObject bullet = Instantiate(pillarBulletPrefab, spawnPos, Quaternion.identity);
            ApplyConfigToBullet(bullet, pillarConfig);
        }

        PlayAttackSound();
    }

    private void ApplyConfigToBullet(GameObject bullet, BulletConfig config)
    {
        IEnemyBullet b = bullet.GetComponent<IEnemyBullet>();
        if (b != null && config != null)
        {
            b.ApplyConfig(config);
        }
    }

    private void PlayAttackSound()
    {
        if (attackSoundKeys != null && attackSoundKeys.Length > 0 && GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = attackSoundKeys[soundIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);

            soundIndex = (soundIndex + 1) % attackSoundKeys.Length;
        }
    }
}



