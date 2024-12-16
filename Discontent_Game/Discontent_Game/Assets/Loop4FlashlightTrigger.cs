using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loop4FlashlightTrigger : MonoBehaviour
{
    [Header("Player & UI Settings")]
    public GameObject player; // Assign the player object in the Inspector
    public GameObject interactText; // Assign the interact text UI object in the Inspector

    [Header("Flashlight Settings")]
    public GameObject flashlightFeatureObject; // The object that contains the Flashlight_Feature script
    public GameObject animationTarget; // Object to play the animation on
    public string animationName; // Name of the animation to play
    public AudioClip pickupSound; // Sound to play on pickup

    [Header("Audio Settings")]
    public float audioVolume = 0.8f; // Volume for the pickup sound

    private bool isPlayerColliding = false; // Tracks if the player is colliding
    private AudioSource audioSource; // Audio source to play the sound
    private Flashlight_Feature flashlightFeature; // Reference to the Flashlight_Feature script

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false); // Hide interact text at start
        }

        // Create or get an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Cache the Flashlight_Feature component
        if (flashlightFeatureObject != null)
        {
            flashlightFeature = flashlightFeatureObject.GetComponent<Flashlight_Feature>();
            if (flashlightFeature != null)
            {
                flashlightFeature.enabled = false; // Disable the feature at start
            }
            else
            {
                Debug.LogError("No Flashlight_Feature script found on the specified flashlightFeatureObject.");
            }
        }
        else
        {
            Debug.LogError("FlashlightFeatureObject is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (isPlayerColliding && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithFlashlight();
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

    private void InteractWithFlashlight()
    {
        if (flashlightFeature != null)
        {
            flashlightFeature.enabled = true; // Enable the Flashlight_Feature script
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

        Debug.Log("Flashlight interaction completed!");
    }
}
