using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Loop4FlashlightTrigger : MonoBehaviour
{
    [Header("Player & UI Settings")]
    public GameObject player; // Assign the player object in the Inspector
    public GameObject interactText; // Assign the interact text UI object in the Inspector

    [Header("Flashlight Settings")]
    public GameObject flashlightObject; // The flashlight object to be disabled
    public GameObject animationTarget; // Object to play the animation on
    public string animationName; // Name of the animation to play
    public AudioClip pickupSound; // Sound to play on pickup

    [Header("Audio Settings")]
    public float audioVolume = 0.8f; // Volume for the pickup sound

    private bool isPlayerColliding = false; // Tracks if the player is colliding
    private AudioSource audioSource; // Audio source to play the sound

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false); // Hide interact text at start
        }

        // Create or get an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isPlayerColliding && Input.GetKeyDown(KeyCode.E))
        {
            PickupFlashlight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerColliding = true;
            if (interactText != null)
            {
                interactText.SetActive(true); // Show interact text
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerColliding = false;
            if (interactText != null)
            {
                interactText.SetActive(false); // Hide interact text
            }
        }
    }

    private void PickupFlashlight()
    {
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false); // Disable the flashlight object
        }

        if (animationTarget != null)
        {
            Animator animator = animationTarget.GetComponent<Animator>();
            if (animator != null && !string.IsNullOrEmpty(animationName))
            {
                animator.Play(animationName); // Play the specified animation
            }
        }

        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.volume = audioVolume;
            audioSource.Play(); // Play the pickup sound
        }

        if (interactText != null)
        {
            interactText.SetActive(false); // Hide interact text after interaction
        }

        Debug.Log("Flashlight picked up!");
    }
}
