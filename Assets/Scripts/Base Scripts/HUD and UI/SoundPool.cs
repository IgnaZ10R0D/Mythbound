using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab; // Prefab asignado con todo configurado

    [Header("Initial Clips")]
    [SerializeField] private List<AudioClip> initialClips;
    [SerializeField] private List<string> keys; // Deben tener el mismo orden que initialClips

    private Dictionary<string, AudioClip> clipsDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> activeSources = new List<AudioSource>();
    private List<AudioSource> inactiveSources = new List<AudioSource>();

    private void Awake()
    {
        for (int i = 0; i < Mathf.Min(initialClips.Count, keys.Count); i++)
        {
            if (!clipsDict.ContainsKey(keys[i]))
                clipsDict.Add(keys[i], initialClips[i]);
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioSource source = CreateNewAudioSource();
            inactiveSources.Add(source);
        }
    }

    public void Play(string key, float volume = 1f)
    {
        if (!clipsDict.ContainsKey(key)) return;

        AudioClip clip = clipsDict[key];
        AudioSource source = GetAvailableSource();
        source.volume = volume;
        source.PlayOneShot(clip);

        activeSources.Add(source);
        StartCoroutine(DisableAfterClip(source, clip.length));
    }

    private AudioSource GetAvailableSource()
    {
        AudioSource source;
        if (inactiveSources.Count > 0)
        {
            source = inactiveSources[0];
            inactiveSources.RemoveAt(0);
        }
        else
        {
            source = CreateNewAudioSource();
        }

        return source;
    }

    private AudioSource CreateNewAudioSource()
    {
        if (audioSourcePrefab == null)
        {
            GameObject go = new GameObject("PooledAudioSource");
            go.transform.SetParent(transform);
            AudioSource fallback = go.AddComponent<AudioSource>();
            fallback.playOnAwake = false;
            return fallback;
        }

        AudioSource source = Instantiate(audioSourcePrefab, transform);
        source.playOnAwake = false;
        return source;
    }

    private IEnumerator DisableAfterClip(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeSources.Remove(source);
        if (!inactiveSources.Contains(source))
            inactiveSources.Add(source);
    }
}


