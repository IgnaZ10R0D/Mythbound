using UnityEngine;

public class ToggleOptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;   // Panel de opciones
    [SerializeField] private GameObject mainPanel;      // Otro panel (como el menú principal)

    private bool isOptionsOpen = false;

    public void ToggleMenu()
    {
        isOptionsOpen = !isOptionsOpen;

        optionsPanel.SetActive(isOptionsOpen);
        mainPanel.SetActive(!isOptionsOpen);
    }
}

