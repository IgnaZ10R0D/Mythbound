using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SomethingChangedText : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float flashSpeed = 8f; 

    private Text text;
    private Color baseColor;

    void Awake()
    {
        text = GetComponent<Text>();
        if (text == null)
        {
            enabled = false;
            return;
        }

        baseColor = text.color;
        text.enabled = false;   // empieza desactivado
    }

    void OnEnable()
    {
        LevelController.OnSomethingChanged += ShowText;
    }

    void OnDisable()
    {
        LevelController.OnSomethingChanged -= ShowText;
    }

    private void ShowText()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        text.enabled = true;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            text.color = Color.Lerp(baseColor, Color.red, t);

            yield return null;
        }

        text.color = baseColor;
        text.enabled = false;
    }
}
