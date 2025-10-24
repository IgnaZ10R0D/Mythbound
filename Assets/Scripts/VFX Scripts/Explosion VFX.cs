using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect; 
    [SerializeField] private AudioSource audioSource; 
    private new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }

        if (particleSystem != null)
        {
            Destroy(gameObject, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
    }
}
