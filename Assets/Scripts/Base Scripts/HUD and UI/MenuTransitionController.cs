using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuTransitionController : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private RectTransform mainMenuPanel;
    [SerializeField] private CanvasGroup fadeOverlay;

    [Header("Paneles deslizables")]
    [SerializeField] private List<GameObject> slidingPanels; 
    
    [Header("Animación")]
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private Vector2 slideOffset = new Vector2(-2000f, 0f);

    private bool isTransitioning = false;

    private void Awake()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
        }
    }

    public void TransitionToScene(string sceneName)
    {
        if (!isTransitioning)
            StartCoroutine(SceneTransitionRoutine(sceneName));
    }

    public void TransitionToPanel(GameObject fromPanel, GameObject toPanel)
    {
        if (!isTransitioning)
            StartCoroutine(PanelTransitionRoutine(fromPanel, toPanel));
    }

    private IEnumerator SceneTransitionRoutine(string sceneName)
    {
        isTransitioning = true;

        mainMenuPanel.gameObject.SetActive(true);

        // Espera un frame para que el click se registre
        yield return null;

        Coroutine slide = null;
        if (slidingPanels.Contains(mainMenuPanel.gameObject))
            slide = StartCoroutine(SlideOut(mainMenuPanel, slideOffset));

        Coroutine fade = null;
        if (fadeOverlay != null)
        {
            fadeOverlay.blocksRaycasts = true;
            fade = StartCoroutine(FadeIn());
        }

        if (slide != null) yield return slide;
        if (fade != null) yield return fade;

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PanelTransitionRoutine(GameObject fromPanel, GameObject toPanel)
    {
        isTransitioning = true;

        bool useSlide = slidingPanels.Contains(fromPanel) || slidingPanels.Contains(toPanel);

        if (useSlide)
        {
            Vector2 direction = slideOffset;
            if (toPanel == mainMenuPanel.gameObject)
                direction = -slideOffset;

            yield return StartCoroutine(SlideOut(fromPanel.GetComponent<RectTransform>(), direction));
            fromPanel.SetActive(false);

            RectTransform toRect = toPanel.GetComponent<RectTransform>();
            toRect.anchoredPosition = -direction;
            toPanel.SetActive(true);

            yield return StartCoroutine(SlideIn(toRect));
        }
        else
        {
            fromPanel.SetActive(false);
            toPanel.SetActive(true);
        }

        isTransitioning = false;
    }

    private IEnumerator SlideOut(RectTransform panel, Vector2 offset)
    {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 endPos = startPos + offset;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / slideDuration;
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
    }

    private IEnumerator SlideIn(RectTransform panel)
    {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 endPos = Vector2.zero;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / slideDuration;
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float alpha = fadeOverlay.alpha;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime / fadeDuration;
            fadeOverlay.alpha = Mathf.Clamp01(alpha);
            yield return null;
        }
    }
}



