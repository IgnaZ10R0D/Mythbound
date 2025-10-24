using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public PlayerHealth player; 
    [SerializeField] private GameObject gameOverPanel; 

    public void ContinueGame()
    {
        if (player.continuesRemaining > 0)
        {
            player.UseContinue();
            Time.timeScale = 1; 
            gameOverPanel.SetActive(false); 
        }
    }
}

