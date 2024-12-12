using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    [Header("Player and Animation Settings")]
    public GameObject playerObject; // The object to consider as the player
    public GameObject targetObject; // The object with the Animator component
    public string animationName = "TriggerAnimation"; // The name of the animation to play
    public AudioClip animationSound; // Sound to play when the animation triggers

    private Animator targetAnimator;
    private AudioSource audioSource;
    private bool hasPlayed = false; // Tracks whether the animation has already been played

    void Start()
    {
        // Ensure the target object has an Animator component
        if (targetObject != null)
        {
            targetAnimator = targetObject.GetComponent<Animator>();
            if (targetAnimator == null)
            {
                Debug.LogError("Target object does not have an Animator component!");
            }
        }
        else
        {
            Debug.LogError("Target object is not assigned!");
        }

        // Ensure the player object is assigned
        if (playerObject == null)
        {
            Debug.LogError("Player object is not assigned!");
        }

        // Set up the audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the specified player object
        if (other.gameObject == playerObject && !hasPlayed)
        {
            // Trigger the animation if the target object and animation name are valid
            if (targetAnimator != null && !string.IsNullOrEmpty(animationName))
            {
                targetAnimator.Play(animationName);
                Debug.Log($"Animation '{animationName}' triggered on {targetObject.name}.");
            }

            // Play the sound if assigned
            if (animationSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(animationSound);
                Debug.Log("Animation sound played.");
            }

            // Mark as played to prevent future triggers
            hasPlayed = true;
        }
    }
}
