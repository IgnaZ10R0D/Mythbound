using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GamePauseManager : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject firstSelectedButton;

    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = EventSystem.current;
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (GameStateController.Instance == null)
            return;

        if (Input.GetKeyDown(pauseKey))
        {
            HandlePauseInput();
        }
    }

    private void HandlePauseInput()
    {
        GameStateController gsc = GameStateController.Instance;

        if (gsc.CurrentState == GameState.Playing)
        {
            bool accepted = gsc.RequestPause();
            if (accepted)
                EnterPauseUI();
        }
        else if (gsc.CurrentState == GameState.Paused)
        {
            gsc.ResumeFromPause();
            ExitPauseUI();
        }
    }

    // =========================
    // UI HANDLING
    // =========================

    private void EnterPauseUI()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        StartCoroutine(SelectButtonNextFrame());
    }

    private void ExitPauseUI()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (eventSystem != null)
            eventSystem.SetSelectedGameObject(null);
    }

    private IEnumerator SelectButtonNextFrame()
    {
        if (eventSystem == null)
            yield break;

        yield return null;

        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(firstSelectedButton);
    }
}