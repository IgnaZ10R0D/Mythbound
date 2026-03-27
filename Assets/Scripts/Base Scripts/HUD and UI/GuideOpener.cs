using UnityEngine;
using System.Collections;

public class GuideOpener : MonoBehaviour
{
    [SerializeField] private CanvasFader mainMenuFader;
    [SerializeField] private CanvasFader guideFader;
    [SerializeField] private GuidePager guidePager;

    public void OpenGuide()
    {
        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        guideFader.gameObject.SetActive(true);

        yield return mainMenuFader.FadeOut();
        yield return guideFader.FadeIn();
    }

    public IEnumerator CloseGuide()
    {
        mainMenuFader.gameObject.SetActive(true);

        yield return guideFader.FadeOut();
        yield return mainMenuFader.FadeIn();
        guidePager.ResetGuide();
    }
}
