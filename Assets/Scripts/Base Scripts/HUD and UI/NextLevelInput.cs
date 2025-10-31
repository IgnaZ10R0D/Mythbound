using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para manipular el texto opcional

public class NextLevelInput : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Min time before advancing")]
    public float delayBeforeAllow = 1.5f;

    [Tooltip("Blinking Text")]
    public Text continueText;
    public float blinkSpeed = 1f;

    private bool canProceed = false;
    private KeyCode shootKey;

    void Start()
    {
        shootKey = InputManager.Instance.GetKey("Shoot");

        if (continueText != null)
            continueText.enabled = false;

        Invoke(nameof(AllowProceed), delayBeforeAllow);
    }

    void Update()
    {
        if (!canProceed)
            return;

        if (continueText != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, alpha);
        }

        if (Input.GetKeyDown(shootKey))
        {
            LoadNextLevel();
        }
    }

    void AllowProceed()
    {
        canProceed = true;
        if (continueText != null)
            continueText.enabled = true;
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}