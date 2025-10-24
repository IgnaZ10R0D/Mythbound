using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtraStageButton : MonoBehaviour
{
    [SerializeField] private Button extraStageButton; 
    [SerializeField] private Image buttonImage; 
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
    private Color originalColor;

    private void Start()
    {
        originalColor = buttonImage.color; 

        if (GameManager.Instance.IsExtraStageUnlocked())
        {
            extraStageButton.interactable = true;
            buttonImage.color = originalColor; 
            extraStageButton.onClick.AddListener(LoadExtraStage);
        }
        else
        {
            extraStageButton.interactable = false;
            buttonImage.color = disabledColor;
        }
    }

    public void LoadExtraStage()
    {
        SceneManager.LoadScene("ExtraStage");
    }
}

