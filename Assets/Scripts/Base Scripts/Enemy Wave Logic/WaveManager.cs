using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public List<CurrentWave> currentWaves = new List<CurrentWave>();
    public GameObject bossDialogueObject;

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
            {
                wave.gameObject.SetActive(false);
            }
        }

        if (bossDialogueObject != null)
        {
            bossDialogueObject.SetActive(false);
        }
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
                {
                    dm.BeginDialogue();
                }
                else
                {
                    Debug.LogWarning("No se encontró DialogueManager en el objeto del jefe.");
                }
            }
        }

        if (currentWaves.Count == 0 && !levelCleared)
        {
            levelCleared = true;
            ClearedStage();
        }
    }

    public void ActivateNextWave()
    {
        if (currentWaves.Count > 0 && currentWaves[0] != null)
        {
            currentWaves[0].gameObject.SetActive(true);
        }
    }

    private void RemoveNullWaves()
    {
        currentWaves.RemoveAll(wave => wave == null);
    }

    public void OnWaveDestroyed(CurrentWave wave)
    {
        if (currentWaves.Contains(wave))
        {
            currentWaves.Remove(wave);
        }

        if (currentWaves.Count > 0)
        {
            ActivateNextWave();
        }
    }

    void ClearedStage()
    {
        Invoke("LoadNextScene", 5f);
    }

    void LoadNextScene()
    {
        levelCleared = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void StartBossFight()
    {
        if (!bossFightStarted)
        {
            bossFightStarted = true;
            musicManager.PlayNextTrack();
            Debug.Log("Boss Fight Started via DialogueManager");
        }
    }
}




