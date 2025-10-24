using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public int totalScore = 0;
    private static GameManager instance;
    public static GameManager Instance => instance;

    private const string ExtraStageKey = "isExtraStageUnlocked";
    private bool isExtraStageUnlocked;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isExtraStageUnlocked = PlayerPrefs.GetInt(ExtraStageKey, 0) == 1;
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
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            ResetPointsToDefault();
            ResetTotalScore();
            if (this != null && gameObject != null && isActiveAndEnabled)
            {
                StartCoroutine(RestoreDefaultUISelection());
            }
        }
    }

    public void ClearedMainStory()
    {
        isExtraStageUnlocked = true;
        PlayerPrefs.SetInt(ExtraStageKey, 1);
        PlayerPrefs.Save();
    }

    public bool IsExtraStageUnlocked() => isExtraStageUnlocked;

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





