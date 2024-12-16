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

    [Header("Door Settings")]
    public GameObject doorObject; // Door object with Animator
    public string doorAnimationName; // Name of the door animation to play
    public AudioClip doorSound; // Assignable audio file to play for the door
    public float doorSoundVolume = 1.0f; // Volume for the door sound

    [Header("Other Settings")]
    public GameObject objectToDisable; // Object to disable after interaction

    private bool isPlayerColliding = false; // Tracks if the player is colliding
    private Flashlight_Feature flashlightFeature; // Reference to the Flashlight_Feature script
    private AudioSource audioSource; // Internal AudioSource for playing sounds

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false); // Hide interact text at start
        }

        // Create or get an AudioSource component for playing sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Cache the Flashlight_Feature component
        if (flashlightFeatureObject != null)
        {
            flashlightFeature = flashlightFeatureObject.GetComponent<Flashlight_Feature>();
            if (flashlightFeature != null)
            {
                flashlightFeature.enabled = false; // Disable the flashlight feature at start
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
            InteractWithFlashlightAndDoor();
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

    private void InteractWithFlashlightAndDoor()
    {
        // Enable the Flashlight_Feature script
        if (flashlightFeature != null)
        {
            flashlightFeature.enabled = true;
        }

        // Play the door animation
        if (doorObject != null)
        {
            Animator animator = doorObject.GetComponent<Animator>();
            if (animator != null && !string.IsNullOrEmpty(doorAnimationName))
            {
                animator.Play(doorAnimationName); // Play the specified animation
            }
            else
            {
                Debug.LogError("Animator or animation name is missing on the door object.");
            }
        }

        // Play the assigned door sound
        if (doorSound != null)
        {
            audioSource.clip = doorSound;
            audioSource.volume = doorSoundVolume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No door sound assigned!");
        }

        // Hide the interact text
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        // Disable the specified object
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        Debug.Log("Flashlight and door interaction completed!");
    }
}
