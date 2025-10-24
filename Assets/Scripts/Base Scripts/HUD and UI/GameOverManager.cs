using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    public void ActivateGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }
}
