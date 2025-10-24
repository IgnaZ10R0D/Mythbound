using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> soundClips; 
    [SerializeField]
    private List<string> soundKeys = new List<string> 
    {
        "UsePassiveSpell",
        "UseActiveSpell",
        "BasicAttack",
        "LoseALife"
    };

    private Dictionary<string, AudioClip> sounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sounds = new Dictionary<string, AudioClip>();

        for (int i = 0; i < soundKeys.Count; i++)
        {
            if (i < soundClips.Count)
            {
                sounds.Add(soundKeys[i], soundClips[i]);
            }
        }
    }

    public void PlaySound(string soundKey)
    {
        if (sounds.ContainsKey(soundKey) && audioSource != null)
        {
            audioSource.PlayOneShot(sounds[soundKey]);
        }
    }
    public string[] GetAvailableKeys()
    {
        if (sounds == null) return new string[0]; 
        List<string> keys = new List<string>(sounds.Keys);
        return keys.ToArray();
    }
}

