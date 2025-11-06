using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StartGame : MonoBehaviour
{
    [SerializeField] private MenuTransitionController transitionController;

    public void OnClickStartGame()
    {
        StartCoroutine(ExecuteAfterFrame(() => 
            transitionController.TransitionToScene("Stage 1")
        ));
    }

    private IEnumerator ExecuteAfterFrame(System.Action action)
    {
        yield return null; 
        action?.Invoke();
    }

    private void OnEnable()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}


