using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 1.5f; 
    [SerializeField] private WaveManager waveManager; 

    private void Start()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();

        if (waveManager == null)
            waveManager = FindFirstObjectByType<WaveManager>();

        StartCoroutine(FadeOutAndStartLevel());
    }

    public IEnumerator FadeOutAndStartLevel()
    {
        if (fadeImage == null)
            yield break;

        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;

        if (waveManager != null && waveManager.currentWaves.Count > 0)
        {
            waveManager.ActivateNextWave();
        }
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage == null)
            yield break;

        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }
}



