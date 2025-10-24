using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth; 
    [SerializeField] private Text healthText;          


    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = $"Lives: {playerHealth.LivesRemaining}";
        }
    }
}

