using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect; 
    [SerializeField] private AudioSource audioSource; 
    private new ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }

        if (_particleSystem != null)
        {
            Destroy(gameObject, _particleSystem.main.duration + _particleSystem.main.startLifetime.constantMax);
        }
    }
}
