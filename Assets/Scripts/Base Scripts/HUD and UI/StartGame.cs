using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StartGame : MonoBehaviour
{
    [SerializeField] private MenuTransitionController transitionController;

    [SerializeField] private GameObject eventSystemPrefab;

    public void OnClickStartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        yield return null;

        EnsureAndResetEventSystem();

        transitionController.TransitionToScene("Stage 1");
    }

    private void EnsureAndResetEventSystem()
    {
        EventSystem es = EventSystem.current;

        if (es == null)
            es = FindFirstObjectByType<EventSystem>();

        if (es == null && eventSystemPrefab != null)
        {
            GameObject go = Instantiate(eventSystemPrefab);
            es = go.GetComponent<EventSystem>();
        }

        if (es == null)
            return;

        es.SetSelectedGameObject(null);

        var modules = es.GetComponents<BaseInputModule>();
        foreach (var module in modules)
        {
            module.enabled = false;
            module.enabled = true;
        }

        var esGO = es.gameObject;
        if (esGO.activeSelf)
        {
            esGO.SetActive(false);
            esGO.SetActive(true);
        }

        StartCoroutine(SelectOnNextFrame(this.gameObject));
    }

    private IEnumerator SelectOnNextFrame(GameObject toSelect)
    {
        yield return null; 
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(toSelect);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SelectOnNextFrame(this.gameObject));
    }
}



