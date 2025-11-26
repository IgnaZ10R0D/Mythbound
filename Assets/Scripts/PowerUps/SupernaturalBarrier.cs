using UnityEngine;

public class SupernaturalBarrier : PowerUp
{
    [Header("Barrier Configuration")]
    public float damageAmount = 50f;
    public float radius = 5f;
    public ParticleSystem barrierEffectPrefab; 

    [Header("Audio Settings")]
    [SerializeField] private string[] soundKeys;
    private int currentSoundIndex = 0;

    private bool isUsed = false;
    private PowerUpController powerUpController;
    private Transform playerTransform;

    void Start()
    {
        powerUpController = FindFirstObjectByType<PowerUpController>();
        playerTransform = FindFirstObjectByType<Player>()?.transform;
    }

    public override void UsePowerUp()
    {
        if (isUsed || playerTransform == null) return;

        isUsed = true;

        if (barrierEffectPrefab != null)
        {
            ParticleSystem ps = Instantiate(barrierEffectPrefab, playerTransform.position, Quaternion.identity);
            ps.Play();

            var shape = ps.shape;
            shape.radius = 0f; 
            StartCoroutine(ExpandParticleShape(ps, radius, 0.5f)); 
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<IEnemyBullet>() != null)
            {
                Destroy(collider.gameObject);
            }
            else if (collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().TakeDamage(damageAmount);
            }
        }

        PlaySound();

        isUsed = false;
    }

    private System.Collections.IEnumerator ExpandParticleShape(ParticleSystem ps, float targetRadius, float duration)
    {
        float elapsed = 0f;
        var shape = ps.shape;
        float initialRadius = shape.radius;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            shape.radius = Mathf.Lerp(initialRadius, targetRadius, elapsed / duration);
            yield return null;
        }

        shape.radius = targetRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemyBullet>() != null && powerUpController != null)
        {
            powerUpController.UsePowerUp<SupernaturalBarrier>();
        }
    }

    private void PlaySound()
    {
        if (soundKeys != null && soundKeys.Length > 0 && GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = soundKeys[currentSoundIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);
            currentSoundIndex = (currentSoundIndex + 1) % soundKeys.Length;
        }
    }
}




