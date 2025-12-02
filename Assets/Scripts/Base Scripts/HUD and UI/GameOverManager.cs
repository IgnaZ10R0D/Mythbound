using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject firstSelectedButton; 

    public void ActivateGameOver()
    {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        ForceSelectButton();
    }

    private void ForceSelectButton()
    {
        if (EventSystem.current == null || firstSelectedButton == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}

