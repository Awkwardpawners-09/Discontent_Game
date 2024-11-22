using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Space; // Set the key to toggle
    public GameObject targetObject; // The object to toggle on and off
    public AudioClip toggleSound; // Assign the sound clip in the Inspector

    private AudioSource audioSource;
    private bool isActive = true;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("No target object assigned!");
            return;
        }

        // Add an AudioSource component if it doesn't already exist
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the sound clip to the AudioSource
        if (toggleSound != null)
        {
            audioSource.clip = toggleSound;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && targetObject != null)
        {
            // Toggle the active state of the target object
            isActive = !isActive;
            targetObject.SetActive(isActive);

            audioSource.Play();
        }
    }
}
