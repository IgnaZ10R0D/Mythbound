using UnityEngine;
using System.Collections.Generic;

public class ShotgunEnemyShot : MonoBehaviour, IAttack
{
    [System.Serializable]
    public class BulletEntry
    {
        public GameObject prefab;
        public BulletConfig config;
    }

    [Header("Shotgun Settings")]
    [SerializeField] private List<BulletEntry> bulletVariants = new List<BulletEntry>();
    [SerializeField] private int bulletsPerShot = 3;
    [SerializeField] private float spreadAngle = 45f;

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
        if (bulletVariants.Count == 0)
        {
            Debug.LogWarning("ShotgunEnemyShot: No hay variantes de balas configuradas.");
            return;
        }

        SpawnShotgunBullets();
        currentVariantIndex = (currentVariantIndex + 1) % bulletVariants.Count;

        PlayAttackSound();
    }

    private void SpawnShotgunBullets()
    {
        BulletEntry current = bulletVariants[currentVariantIndex];

        if (bulletsPerShot <= 1)
        {
            // Disparo único
            SpawnSingleBullet(current, transform.rotation.eulerAngles.z);
            return;
        }

        float angleStep = spreadAngle / (bulletsPerShot - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = startAngle + (i * angleStep);
            SpawnSingleBullet(current, angle);
        }
    }

    private void SpawnSingleBullet(BulletEntry entry, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject bullet = Instantiate(entry.prefab, transform.position, rotation);
        ApplyConfigToBullet(bullet, entry.config);
    }

    private void ApplyConfigToBullet(GameObject bullet, BulletConfig config)
    {
        IEnemyBullet bulletScript = bullet.GetComponent<IEnemyBullet>();
        if (bulletScript != null && config != null)
        {
            bulletScript.ApplyConfig(config);
        }
        else
        {
            Debug.LogWarning("ShotgunEnemyShot: La bala no tiene IEnemyBullet o falta config.");
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



