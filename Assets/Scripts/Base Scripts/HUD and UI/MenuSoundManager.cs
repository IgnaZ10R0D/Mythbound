using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSoundManager : MonoBehaviour
{
    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip selectClip;  

    private GameObject _lastSelected;

    void Update()
    {
        if (EventSystem.current == null) return;

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && Input.GetButtonDown("Submit"))
        {
            PlaySelectSound();
        }
    }
    
    private void PlaySelectSound()
    {
        if (audioSource != null && selectClip != null)
            audioSource.PlayOneShot(selectClip);
    }
}

