using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public int totalScore = 0;
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private const string ExtraStageKey = "isExtraStageUnlocked";
    private bool _isExtraStageUnlocked;

    // ---------- RUN STATE ----------
    private Dictionary<string, int> levelKills = new();
    private Dictionary<string, bool> levelFlags = new();

    private bool _badEndUnlocked = false;

    public bool BadEndUnlocked => _badEndUnlocked;

    private const int RequiredLevelsForBadEnd = 3;

    // --------------------------------

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _isExtraStageUnlocked = PlayerPrefs.GetInt(ExtraStageKey, 0) == 1;
        }
        else
        {
            Destroy(gameObject);
        }

        totalScore = 0;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            ResetRunState();
            ResetPointsToDefault();
            ResetTotalScore();

            if (this != null && gameObject != null && isActiveAndEnabled)
            {
                StartCoroutine(RestoreDefaultUISelection());
            }
        }
    }

    // ---------- RUN STATE METHODS ----------

    public void RegisterEnemyKill(string levelID)
    {
        if (!levelKills.ContainsKey(levelID))
            levelKills[levelID] = 0;

        levelKills[levelID]++;
    }

    public int GetKills(string levelID)
    {
        if (!levelKills.ContainsKey(levelID))
            return 0;

        return levelKills[levelID];
    }

    public void ActivateSomethingChanged(string levelID)
    {
        if (!levelFlags.ContainsKey(levelID))
            levelFlags[levelID] = false;

        if (levelFlags[levelID])
            return;

        levelFlags[levelID] = true;

        EvaluateBadEnd();
    }

    public bool HasSomethingChanged(string levelID)
    {
        return levelFlags.ContainsKey(levelID) && levelFlags[levelID];
    }

    private void EvaluateBadEnd()
    {
        int count = levelFlags.Values.Count(v => v);

        if (count >= RequiredLevelsForBadEnd)
        {
            _badEndUnlocked = true;
        }
    }

    public void ResetRunState()
    {
        levelKills.Clear();
        levelFlags.Clear();
        _badEndUnlocked = false;
    }

    // ---------------------------------------

    public void ClearedMainStory()
    {
        _isExtraStageUnlocked = true;
        PlayerPrefs.SetInt(ExtraStageKey, 1);
        PlayerPrefs.Save();
    }

    public bool IsExtraStageUnlocked() => _isExtraStageUnlocked;

    public void ResetTotalScore()
    {
        totalScore = 0;
        SaveTotalScore();
    }

    public void SaveTotalScore()
    {
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
    }

    private void ResetPointsToDefault()
    {
        PointManager pointManager = FindFirstObjectByType<PointManager>();
        if (pointManager != null)
        {
            pointManager.ResetPoints();
        }
    }

    private IEnumerator RestoreDefaultUISelection()
    {
        while (EventSystem.current == null)
            yield return null;

        yield return null;

        Selectable[] selectables = null;
        while (selectables == null || selectables.Length == 0)
        {
            selectables = GameObject.FindObjectsByType<Selectable>(FindObjectsSortMode.None)
                .Where(s => s != null && s.IsActive() && s.IsInteractable())
                .ToArray();
            yield return null;
        }

        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject == null &&
            selectables.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        }
    }

    public void QuitGame()
    {
        ResetTotalScore();
        ResetPointsToDefault();
        Application.Quit();
    }
}
