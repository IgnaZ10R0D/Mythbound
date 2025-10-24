using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> soundClips; 
    [SerializeField]
    private List<string> soundKeys = new List<string> 
    {
        "TakeDamage",
        "Attack1",
        "Attack2",
        "Attack3",
        "LongAttack"
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
}

