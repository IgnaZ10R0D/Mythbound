using UnityEngine;
using UnityEngine.UI;

public class ControlTextDisplay : MonoBehaviour
{
    [SerializeField] private Text targetText; 
    [SerializeField] private string displayName;  
    [SerializeField] private string actionName;   

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (targetText == null)
        {
            Debug.LogWarning("No se asignó un Text en ControlTextDisplay.");
            return;
        }

        KeyCode key = InputManager.Instance.GetKey(actionName);

        targetText.text = $"{displayName}: {key}";
    }
}

