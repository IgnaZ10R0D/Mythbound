using UnityEngine;

public class SupernaturalBarrier : PowerUp
{
    [Header("Barrier Configuration")]
    public float damageAmount = 50f;
    public float radius = 5f;
    public ParticleSystem barrierEffectPrefab; 

    private bool isUsed = false;
    private PlayerSounds playerSounds;
    private PowerUpController powerUpController;
    private Transform playerTransform;

    void Start()
    {
        powerUpController = FindFirstObjectByType<PowerUpController>();
        playerSounds = FindFirstObjectByType<PlayerSounds>(); 
        playerTransform = FindFirstObjectByType<Player>().transform; 
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

        if (playerSounds != null)
        {
            playerSounds.PlaySound("UsePassiveSpell");
        }

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
}



