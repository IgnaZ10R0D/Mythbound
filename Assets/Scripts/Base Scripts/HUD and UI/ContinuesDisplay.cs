using UnityEngine;
using UnityEngine.UI;

public class ContinuesDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text targetText;

    private void OnEnable()
    {
        if (playerHealth != null && targetText != null)
        {
            targetText.text = $"Continues Remaining - {playerHealth.continuesRemaining}";
        }
    }
}

