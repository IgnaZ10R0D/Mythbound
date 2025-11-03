using UnityEngine;

public class ToggleOptionsMenu : MonoBehaviour
{
    [SerializeField] private MenuTransitionController transitionController;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainPanel;

    private bool isOptionsOpen = false;

    public void ToggleMenu()
    {
        if (isOptionsOpen)
            transitionController.TransitionToPanel(optionsPanel, mainPanel);
        else
            transitionController.TransitionToPanel(mainPanel, optionsPanel);

        isOptionsOpen = !isOptionsOpen;
    }
}


