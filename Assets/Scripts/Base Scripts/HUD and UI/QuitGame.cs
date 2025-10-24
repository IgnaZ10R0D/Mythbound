using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void OnClickQuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para detener el juego en el editor de Unity
#endif
    }
}

