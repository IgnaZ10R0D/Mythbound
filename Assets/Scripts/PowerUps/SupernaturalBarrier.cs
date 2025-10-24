using UnityEngine;

public class SupernaturalBarrier : PowerUp
{
    public float damageAmount = 50f;
    private bool isUsed;
    private PlayerSounds playerSounds;
    private PowerUpController powerUpController;

    void Start()
    {
        powerUpController = FindFirstObjectByType<PowerUpController>();
        playerSounds = GetComponentInParent<PlayerSounds>();
        isUsed = false;
    }

    public override void UsePowerUp()
    {
        if (isUsed) return;

        isUsed = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemyBullet>() != null && powerUpController != null)
        {
            powerUpController.UsePowerUp<SupernaturalBarrier>();
        }
    }
}








