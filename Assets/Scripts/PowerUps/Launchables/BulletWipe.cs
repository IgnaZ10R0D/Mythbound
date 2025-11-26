using UnityEngine;

public class BulletWipe : MonoBehaviour
{
    [Header("Bullet Wipe Settings")]
    [SerializeField] private float expansionSpeed = 10f;
    [SerializeField] private float maxScale = 20f;
    [SerializeField] private string enemyBulletTag = "EnemyBullet";

    [Header("Audio Settings")]
    [SerializeField] private string[] soundKeys;
    private int currentSoundIndex = 0;

    void Start()
    {
        PlaySound();
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