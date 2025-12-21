using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _livesRemaining = 2;
    public int continuesRemaining = 3;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private float spriteDisableTime = 2f;
    [SerializeField] private float colliderDisableTime = 3f;
    [SerializeField] private float gameOverDelay = 1f; 
    [SerializeField] private float transparencyWhenDisabled = 0.5f; 

    [SerializeField] private GameOverManager gameOverManager;
    private SpriteRenderer spRd;
    private Collider2D[] colliders;
    private Transform[] childObjects;

    private int currentSoundIndex = 0;
    [SerializeField] private string[] healthSoundKeys;

    [SerializeField] private int framesToActivateDamage = 5;
    private int currentFramesInTrigger;
    public int LivesRemaining => _livesRemaining;

    private bool isHandlingDamage = false; 

    private void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        childObjects = GetComponentsInChildren<Transform>(true);
        gameOverManager = FindFirstObjectByType<GameOverManager>();
        currentFramesInTrigger = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemyBullet>() != null || collision.GetComponent<Enemy>() != null)
        {
            currentFramesInTrigger = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemyBullet>() != null || collision.GetComponent<Enemy>() != null)
        {
            currentFramesInTrigger++;

            if (currentFramesInTrigger >= framesToActivateDamage && !isHandlingDamage)
            {
                StartCoroutine(HandleDamageAndRespawn());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemyBullet>() != null || collision.GetComponent<Enemy>() != null)
        {
            currentFramesInTrigger = 0;
        }
    }

    private IEnumerator HandleDamageAndRespawn()
    {
        if (isHandlingDamage) yield break;
        isHandlingDamage = true;

        if (healthSoundKeys != null && healthSoundKeys.Length > 0 && GameplaySoundsManager.Instance != null)
        {
            string keyToPlay = healthSoundKeys[currentSoundIndex];
            GameplaySoundsManager.Instance.Play(keyToPlay);
            currentSoundIndex = (currentSoundIndex + 1) % healthSoundKeys.Length;
        }

        if (_livesRemaining > 0) _livesRemaining--;

        yield return StartCoroutine(TemporaryDisableWithRespawn(respawnPosition));

        if (_livesRemaining == 0)
        {
            yield return new WaitForSeconds(gameOverDelay); 
            CheckGameOver();
            isHandlingDamage = false;
            yield break;
        }

        isHandlingDamage = false;
    }

    private IEnumerator TemporaryDisableWithRespawn(Vector3 targetPosition)
    {
        if (spRd != null) spRd.enabled = false;
        if (colliders != null)
        {
            foreach (var col in colliders) if (col != null) col.enabled = false;
        }
        SetChildrenActive(false);

        transform.position = targetPosition;

        yield return new WaitForSeconds(spriteDisableTime);

        if (_livesRemaining > 0)
        {
            if (spRd != null) spRd.enabled = true;
            SetChildrenActive(true);
        }

        yield return new WaitForSeconds(Mathf.Max(0f, colliderDisableTime - spriteDisableTime));

        if (colliders != null)
        {
            foreach (var col in colliders) if (col != null) col.enabled = true;
        }
    }

    private void CheckGameOver()
    {
        if (gameOverManager != null)
            gameOverManager.ActivateGameOver();
    }

    private void SetChildrenActive(bool isActive)
    {
        if (childObjects == null) return;

        foreach (Transform child in childObjects)
        {
            if (child != null && child != transform)
                child.gameObject.SetActive(isActive);
        }
    }

    public void LifeGet()
    {
        _livesRemaining++;
    }

    public void UseContinue()
    {
        if (continuesRemaining > 0)
        {
            continuesRemaining--;
            _livesRemaining = 2; 
            StartCoroutine(TemporaryDisableWithRespawn(respawnPosition));
        }
    }

    private void Update()
    {
        if (spRd == null || colliders == null) return;

        bool anyColliderActive = false;
        foreach (var col in colliders)
        {
            if (col != null && col.enabled)
            {
                anyColliderActive = true;
                break;
            }
        }

        Color spriteColor = spRd.color;
        spriteColor.a = anyColliderActive ? 1f : transparencyWhenDisabled;
        spRd.color = spriteColor;
    }
}


