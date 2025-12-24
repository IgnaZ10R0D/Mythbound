using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject firstSelectedButton;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// called only by GameStateController
    /// when the state becomes GameOver.
    /// </summary>
    public void ActivateGameOver()
    {
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


