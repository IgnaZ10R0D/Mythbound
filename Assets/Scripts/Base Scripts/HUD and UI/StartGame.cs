using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Stage 1");
    }
}
