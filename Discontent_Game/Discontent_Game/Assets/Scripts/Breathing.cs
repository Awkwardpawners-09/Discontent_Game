using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public AudioClip breathingSound; // Assign the breathing sound clip in the Inspector
    public float volume = 0.5f; // Adjust volume as needed

    private AudioSource audioSource;

    private void Start()
    {
        // Add and configure the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = breathingSound;
        audioSource.volume = volume;
        audioSource.loop = true; // Enable looping to continuously play the sound
        audioSource.playOnAwake = true; // Automatically start playing on Awake
        audioSource.Play();
    }
}
