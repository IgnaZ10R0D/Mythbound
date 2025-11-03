using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
	[SerializeField] private MenuTransitionController transitionController;
    public void OnClickStartGame()
    {
	    transitionController.TransitionToScene("Stage 1");
    }
	private void OnEnable()
	{
		if (EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
		}
	}
}
