using UnityEngine;

public class BulletWipe : MonoBehaviour
{
    [Header("Bullet Wipe Settings")]
    [SerializeField] private float expansionSpeed = 10f;
    [SerializeField] private float maxScale = 20f;

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

        float radius = transform.localScale.x * 0.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            Bullet bullet = hit.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.gameObject.SetActive(false);
            }
        }

        if (transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        if (soundKeys != null && soundKeys.Length > 0 &&
            GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = soundKeys[currentSoundIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);
            currentSoundIndex = (currentSoundIndex + 1) % soundKeys.Length;
        }
    }
}