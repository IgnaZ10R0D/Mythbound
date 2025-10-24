using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public void ResumeGame()
    {
        Time.timeScale = 1; 
        pausePanel.SetActive(false); 
    }
}

