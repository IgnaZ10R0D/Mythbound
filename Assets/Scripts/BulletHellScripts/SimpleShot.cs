using UnityEngine;

public class SimpleShot : MonoBehaviour, IAttack
{
    [System.Serializable]
    public class BulletEntry
    {
        public GameObject prefab;
        public BulletConfig config;
    }

    [Header("Single Settings")]
    [SerializeField] private BulletEntry bulletEntry; 
    [SerializeField] private Transform firePoint;

    [Header("Audio")]
    [SerializeField] private string[] attackSoundKeys;
    private int currentSoundIndex = 0;

    [Header("Health Phases")]
    [SerializeField] private int[] healthIndexes = { 0, 2 };
    public int[] HealthIndexes => healthIndexes;

    public bool IsContinuous => false;
    public EnemyAttackHandler enemyAttackScript { get; set; }

    private void Start()
    {
        enemyAttackScript = GetComponent<EnemyAttackHandler>();
    }

    public void Attack()
    {
        if (bulletEntry?.prefab == null || bulletEntry.config == null || firePoint == null)
        {
            Debug.LogWarning("SimpleShot: Falta prefab, config o firePoint.");
            return;
        }

        GameObject bullet = Instantiate(bulletEntry.prefab, firePoint.position, firePoint.rotation);
        ApplyConfigToBullet(bullet, bulletEntry.config);

        PlayAttackSound();
    }

    private void ApplyConfigToBullet(GameObject bullet, BulletConfig config)
    {
        IEnemyBullet configurableBullet = bullet.GetComponent<IEnemyBullet>();
        if (configurableBullet != null)
        {
            configurableBullet.ApplyConfig(config);
        }
        else
        {
            Debug.LogWarning("SimpleShot: La bala no tiene un componente IEnemyBullet.");
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



