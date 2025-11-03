using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PowerUpUIController : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconParent;

    [Header("Unlock Text")]
    [SerializeField] private GameObject unlockTextPrefab; 
    [SerializeField] private Transform unlockTextParent;  

    [Header("Unlock Sound")]
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioSource audioSource; 

    private Dictionary<string, Image> activeIcons = new Dictionary<string, Image>();
    private Dictionary<string, bool> hasUnlocked = new Dictionary<string, bool>();
    private PowerUpData[] powerUps;

    private ActiveLauncher currentlySelected;

    private void OnDestroy()
    {
        StopAllCoroutines();
        activeIcons.Clear();
        hasUnlocked.Clear();
        currentlySelected = null;
        powerUps = null;
    }

    public void InitializeIcons(PowerUpData[] powerUps)
    {
        this.powerUps = powerUps;

        foreach (Transform child in iconParent)
            Destroy(child.gameObject);

        activeIcons.Clear();
        hasUnlocked.Clear();

        foreach (var data in powerUps)
        {
            GameObject iconGO = Instantiate(iconPrefab, iconParent);
            Image img = iconGO.GetComponent<Image>();
            img.sprite = data.icon;
            img.color = Color.red * 0.7f;
            activeIcons[data.displayName] = img;

            hasUnlocked[data.displayName] = false;
        }
    }

    public void UpdatePowerUpIcons(int currentPoints)
    {
        bool anyAvailable = false;

        foreach (var data in powerUps)
        {
            if (!activeIcons.ContainsKey(data.displayName)) continue;

            Image icon = activeIcons[data.displayName];
            bool available = currentPoints >= data.thresholdPoints;
            bool wasUnlocked = hasUnlocked[data.displayName];

            if (available && !wasUnlocked)
            {
                hasUnlocked[data.displayName] = true;
                ShowUnlockText(data.displayName);
                PlayUnlockSound();
            }
            else if (!available && wasUnlocked)
            {
                hasUnlocked[data.displayName] = false;
                if (currentlySelected == data.powerUp as ActiveLauncher)
                    currentlySelected = null; 
            }

            icon.color = available ? Color.white : (Color.red * 0.7f);

            if (available)
                anyAvailable = true;
        }

        if (currentlySelected == null && anyAvailable)
        {
            foreach (var data in powerUps)
            {
                if (hasUnlocked[data.displayName] && data.powerUp is ActiveLauncher launcher)
                {
                    currentlySelected = launcher;
                    break;
                }
            }
        }

        if (!anyAvailable)
        {
            currentlySelected = null;
        }

        HighlightSelectedPowerUp(currentlySelected);
    }

    private void ShowUnlockText(string displayName)
    {
        if (unlockTextPrefab == null || unlockTextParent == null) return;

        GameObject textGO = Instantiate(unlockTextPrefab, unlockTextParent);
        Text textComp = textGO.GetComponent<Text>();
        if (textComp != null)
            textComp.text = $"{displayName} Unlocked!";

        StartCoroutine(FadeAndDestroy(textGO, 2f));
    }

    private IEnumerator FadeAndDestroy(GameObject textGO, float duration)
    {
        Text txt = textGO.GetComponent<Text>();
        if (txt == null) yield break;

        Vector3 originalScale = textGO.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            if (txt == null) yield break; 

            timer += Time.deltaTime;
            float t = timer / duration;

            float r = Mathf.Abs(Mathf.Sin(timer * 5f));
            float g = Mathf.Abs(Mathf.Sin(timer * 3f));
            float b = Mathf.Abs(Mathf.Sin(timer * 4f));
            txt.color = new Color(r, g, b, Mathf.Lerp(1f, 0f, t));

            float scaleMultiplier = 1f + 0.5f * Mathf.Sin(timer * 10f);
            textGO.transform.localScale = originalScale * scaleMultiplier;

            yield return null;
        }

        if (textGO != null)
            Destroy(textGO);
    }

    private void PlayUnlockSound()
    {
        if (unlockSound == null) return;

        if (audioSource == null)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.playOnAwake = false;
            temp.spatialBlend = 0f;
            temp.PlayOneShot(unlockSound);
            Destroy(temp, unlockSound.length);
        }
        else
        {
            audioSource.PlayOneShot(unlockSound);
        }
    }

    public void HighlightSelectedPowerUp(ActiveLauncher selectedPowerUp)
    {
        currentlySelected = selectedPowerUp;

        foreach (var data in powerUps)
        {
            if (!activeIcons.ContainsKey(data.displayName)) continue;

            Image icon = activeIcons[data.displayName];

            if (data.powerUp == selectedPowerUp)
                icon.color = Color.yellow; 
            else
                icon.color = hasUnlocked[data.displayName] ? Color.white : (Color.red * 0.7f);
        }
    }

    public void ClearSelection()
    {
        foreach (var data in powerUps)
        {
            if (!activeIcons.ContainsKey(data.displayName)) continue;

            Image icon = activeIcons[data.displayName];
            icon.color = hasUnlocked[data.displayName] ? Color.white : (Color.red * 0.7f);
        }
    }
}



