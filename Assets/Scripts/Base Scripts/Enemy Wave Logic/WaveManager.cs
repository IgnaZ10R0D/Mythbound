using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<CurrentWave> currentWaves = new List<CurrentWave>();

    [Header("Boss & Victory")]
    public GameObject bossDialogueObject;
    public GameObject victoryPanel;

    [Header("Fade")]
    public FadeController fadeController;

    private MusicManager musicManager;
    private GameManager gameManager;

    private bool bossFightStarted = false;
    private bool levelCleared = false;
    private bool dialogueTriggered = false;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        gameManager = FindObjectOfType<GameManager>();

        foreach (CurrentWave wave in currentWaves)
        {
            if (wave != null)
                wave.gameObject.SetActive(false);
        }

        if (bossDialogueObject != null)
            bossDialogueObject.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (fadeController == null)
            fadeController = FindFirstObjectByType<FadeController>();

        if (fadeController != null)
            StartCoroutine(fadeController.FadeOutAndStartLevel());
        else
            ActivateNextWave();
    }

    private void Update()
    {
        RemoveNullWaves();
        
        if (dialogueTriggered && !bossFightStarted &&
            GameStateController.Instance != null &&
            GameStateController.Instance.CurrentState == GameState.Playing)
        {
            StartBossFight();
        }
        if (currentWaves.Count > 0 && !currentWaves[0].gameObject.activeSelf)
        {
            ActivateNextWave();
        }

        // ---------- BOSS DIALOGUE TRIGGER ----------
        if (currentWaves.Count == 1 && !dialogueTriggered)
        {
            dialogueTriggered = true;
            TriggerBossDialogue();
        }

        // ---------- VICTORY ----------
        if (currentWaves.Count == 0 && !levelCleared)
        {
            levelCleared = true;
            ShowVictoryPanel();
        }
    }

    private void TriggerBossDialogue()
    {
        if (bossDialogueObject == null || GameStateController.Instance == null)
            return;

        bossDialogueObject.SetActive(true);
        GameStateController.Instance.StartDialogue();
    }

    public void ActivateNextWave()
    {
        if (currentWaves.Count > 0 && currentWaves[0] != null)
            currentWaves[0].gameObject.SetActive(true);
    }

    private void RemoveNullWaves()
    {
        currentWaves.RemoveAll(wave => wave == null);
    }

    public void OnWaveDestroyed(CurrentWave wave)
    {
        if (currentWaves.Contains(wave))
            currentWaves.Remove(wave);

        if (currentWaves.Count > 0)
            ActivateNextWave();
    }

    public void StartBossFight()
    {
        if (bossFightStarted)
            return;

        bossFightStarted = true;

        if (musicManager != null)
            musicManager.PlayNextTrack();
    }

    private void ShowVictoryPanel()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
}




