using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<CurrentWave> currentWaves = new List<CurrentWave>();
    public GameObject bossDialogueObject;
    public GameObject victoryPanel;
    public FadeController fadeController; // 👈 Nueva referencia al FadeController

    private MusicManager musicManager;
    private GameManager gameManager;
    private bool bossFightStarted = false;
    private bool levelCleared = false;
    private bool dialogueTriggered = false;

    void Start()
    {
        musicManager = FindFirstObjectByType<MusicManager>();
        gameManager = FindFirstObjectByType<GameManager>();

        foreach (CurrentWave wave in currentWaves)
        {
            if (wave != null)
                wave.gameObject.SetActive(false);
        }

        if (bossDialogueObject != null)
            bossDialogueObject.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // 👇 En vez de activar la primera oleada directamente,
        // pedimos al FadeController que haga fade out primero
        if (fadeController == null)
            fadeController = FindFirstObjectByType<FadeController>();

        if (fadeController != null)
            StartCoroutine(fadeController.FadeOutAndStartLevel());
        else
            ActivateNextWave(); // fallback si no hay FadeController
    }

    void Update()
    {
        RemoveNullWaves();

        if (currentWaves.Count > 0 && !currentWaves[0].gameObject.activeSelf)
        {
            ActivateNextWave();
        }

        if (currentWaves.Count == 1 && !bossFightStarted && !dialogueTriggered)
        {
            dialogueTriggered = true;

            if (bossDialogueObject != null)
            {
                bossDialogueObject.SetActive(true);

                DialogueManager dm = bossDialogueObject.GetComponent<DialogueManager>();
                if (dm != null)
                    dm.BeginDialogue();
            }
        }

        if (currentWaves.Count == 0 && !levelCleared)
        {
            levelCleared = true;
            ShowVictoryPanel();
        }
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

    void ShowVictoryPanel()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    public void StartBossFight()
    {
        if (!bossFightStarted)
        {
            bossFightStarted = true;
            musicManager.PlayNextTrack();
        }
    }
}



