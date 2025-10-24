using UnityEngine;

public class BulletWipe : MonoBehaviour
{
    [SerializeField] private float expansionSpeed = 10f;
    [SerializeField] private float maxScale = 20f;
    [SerializeField] private string enemyBulletTag = "EnemyBullet";
    [SerializeField] private string soundName = "UseActiveSpell";

    private PlayerSounds playerSounds;

    void Start()
    {
        
        playerSounds = FindFirstObjectByType<PlayerSounds>();

        if (playerSounds != null && !string.IsNullOrEmpty(soundName))
        {
            playerSounds.PlaySound(soundName);
        }
    }

    void Update()
    {
        transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyBulletTag))
            {
                Destroy(hit.gameObject);
            }
        }

        if (transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }
}






