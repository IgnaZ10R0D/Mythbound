using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject firstSelectedButton;

    private bool isPaused = false;
    private EventSystem eventSystem;

    void Awake()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
    }

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.LivesRemaining <= 0)
            return;

        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            StartCoroutine(SelectButtonNextFrame());
        }
        else
        {
            if (eventSystem != null)
                eventSystem.SetSelectedGameObject(null);
        }
    }

    private IEnumerator SelectButtonNextFrame()
    {
        if (eventSystem == null)
            eventSystem = FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
            yield break; 

        if (!eventSystem.gameObject.activeInHierarchy)
        {
            eventSystem.gameObject.SetActive(true);
            yield return null; 
        }

        yield return null; 

        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(firstSelectedButton);
    }
}




