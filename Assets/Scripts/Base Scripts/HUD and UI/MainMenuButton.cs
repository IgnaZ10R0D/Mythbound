using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public void GoToMainMenu()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu"); 
    }
}

