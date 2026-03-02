using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private GameManager gameManager;
    private static ScoreHandler instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        LoadTotalScore();
        UpdateScoreText();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddScore(int scoreToAdd)
    {
        gameManager.totalScore += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {gameManager.totalScore:D8}";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            gameManager.ResetTotalScore();
            Destroy(gameObject); 
        }
        else
        {
            gameManager.SaveTotalScore();
        }
        UpdateScoreText();
    }

    

    private void LoadTotalScore()
    {
        gameManager.totalScore = PlayerPrefs.GetInt("TotalScore", 0);
    }

    

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

