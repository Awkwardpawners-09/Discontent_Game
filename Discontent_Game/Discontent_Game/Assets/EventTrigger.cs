using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerObject; // Assign the player object in the Inspector

    [Header("Target Settings")]
    public GameObject targetObject; // Assign the target object with the Animator component
    public string animationName = "DefaultAnimation"; // Name of the animation to play
    public AudioClip soundFile; // Audio file to play

    [Header("Countdown Settings")]
    public float countdownDuration = 15f; // Countdown duration in seconds

    private Animator targetAnimator; // Reference to the Animator component
    private AudioSource audioSource; // Reference to the AudioSource component
    private bool isCountingDown = false; // Check if the countdown is active
    private float countdownTimer; // Timer for the countdown

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

        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Failed to create an AudioSource component!");
        }
    }

    void Update()
    {
        // Handle the countdown
        if (isCountingDown)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                TriggerAnimationAndSound();
                isCountingDown = false; // Stop further countdowns
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Start the countdown if the colliding object is the player
        if (other.gameObject == playerObject && !isCountingDown)
        {
            isCountingDown = true;
            countdownTimer = countdownDuration;
            Debug.Log("Countdown started!");
        }
    }

    private void TriggerAnimationAndSound()
    {
        // Play the animation
        if (targetAnimator != null && !string.IsNullOrEmpty(animationName))
        {
            targetAnimator.Play(animationName);
            Debug.Log($"Animation '{animationName}' triggered on {targetObject.name}.");
        }

        // Play the sound
        if (soundFile != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundFile);
            Debug.Log("Sound file played.");
        }
    }
}
