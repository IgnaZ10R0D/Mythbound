using UnityEngine;
using System.Collections.Generic;

public class RingEnemyShot : MonoBehaviour, IAttack
{
    [System.Serializable]
    public class BulletEntry
    {
        public GameObject prefab;
        public BulletConfig config;
    }

    [Header("Bullets")]
    [SerializeField] private List<BulletEntry> bulletVariants = new List<BulletEntry>();
    [SerializeField] private int bulletsPerRing = 8;
    [SerializeField] private float spawnRadius = 2f;

    [Header("Audio")]
    [SerializeField] private string[] attackSoundKeys;
    private int currentSoundIndex = 0;

    [Header("Health Phases")]
    [SerializeField] private int[] healthIndexes = { 0, 2 };
    public int[] HealthIndexes => healthIndexes;

    public bool IsContinuous => false;
    public EnemyAttackHandler enemyAttackScript { get; set; }

    private int currentVariantIndex = 0;

    private void Start()
    {
        enemyAttackScript = GetComponent<EnemyAttackHandler>();
    }

    public void Attack()
    {
        if (bulletVariants.Count == 0) return;

        SpawnRingOfBullets();
        PlayAttackSound();

        currentVariantIndex = (currentVariantIndex + 1) % bulletVariants.Count;
    }

    private void SpawnRingOfBullets()
    {
        float angleStep = 360f / bulletsPerRing;
        BulletEntry current = bulletVariants[currentVariantIndex];

        for (int i = 0; i < bulletsPerRing; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 spawnOffset = new Vector3(
                Mathf.Cos(angleRad) * spawnRadius,
                Mathf.Sin(angleRad) * spawnRadius,
                0f
            );

            Vector3 spawnPosition = transform.position + spawnOffset;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject bullet = Instantiate(current.prefab, spawnPosition, rotation);
            ApplyConfigToBullet(bullet, current.config);
        }
    }

    private void ApplyConfigToBullet(GameObject bullet, BulletConfig config)
    {
        IEnemyBullet configurableBullet = bullet.GetComponent<IEnemyBullet>();
        if (configurableBullet != null && config != null)
        {
            configurableBullet.ApplyConfig(config);
        }
        else
        {
            Debug.LogWarning("Bullet instanciada sin script válido o sin config.");
        }
    }

    private void PlayAttackSound()
    {
        if (attackSoundKeys != null && attackSoundKeys.Length > 0 && GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = attackSoundKeys[currentSoundIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);

            currentSoundIndex = (currentSoundIndex + 1) % attackSoundKeys.Length;
        }
    }
}



