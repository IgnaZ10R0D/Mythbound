using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Slider healthSlider; 

    void Start()
    {
        healthSlider.maxValue = enemy.MaxHealth;
        healthSlider.value = enemy.CurrentHealth;
    }

    void Update()
    {
        if (enemy != null && healthSlider != null)
        {
            healthSlider.value = enemy.CurrentHealth;
        }
    }
}
