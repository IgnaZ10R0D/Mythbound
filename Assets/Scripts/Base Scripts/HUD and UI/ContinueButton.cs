using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueButton : MonoBehaviour
{
    private GameStateController _gameStateController;

    public PlayerHealth player;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        ResolveGameStateController();
    }

    private void OnEnable()
    {
        ResolveGameStateController();

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    private void ResolveGameStateController()
    {
        if (_gameStateController != null)
            return;

        if (GameStateController.Instance != null)
        {
            _gameStateController = GameStateController.Instance;
            return;
        }

        _gameStateController = FindObjectOfType<GameStateController>();
    }

    public void ContinueGame()
    {
        if (player == null)
            return;

        if (player.continuesRemaining <= 0)
            return;

        // 1. Usar continue
        player.UseContinue();

        // 2. Volver al estado Playing desde GameOver
        if (_gameStateController != null)
        {
            _gameStateController.ResumeFromGameOver();
        }
        else
        {
            // Fallback de emergencia (no debería pasar)
            Time.timeScale = 1f;
        }

        // 3. Cerrar panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}



