using UnityEngine;
using System.Collections;

public class TimeWarp : MonoBehaviour
{
    [Tooltip("Factor de velocidad: 1 = normal, 0 = detenido, 2 = doble velocidad")]
    public float timeFactor = 0.5f; 

    [Tooltip("Duración del TimeWarp en segundos")]
    public float duration = 5f; 

    private void Start()
    {
        if (TimeManager.Instance != null)
        {
            StartCoroutine(ApplyTimeWarp());
        }
    }

    private IEnumerator ApplyTimeWarp()
    {
        if (TimeManager.Instance == null)
        {
            yield break;
        }

        TimeManager.Instance.SetTimeSlow(timeFactor);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        TimeManager.Instance.ResetTimeSlow();
        Destroy(gameObject);
    }
}