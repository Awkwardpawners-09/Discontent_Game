using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteTrigger : MonoBehaviour
{
    [Header("Player and Camera")]
    public GameObject player; // Assign the player object
    public MonoBehaviour playerMovementController; // Assign the player's movement script
    public MonoBehaviour cameraController; // Assign the camera script

    [Header("UI Elements")]
    public Text interactionText; // Text to display interaction prompt
    public RawImage rawImage; // The raw image object to display

    [Header("Audio Clips")]
    public AudioClip interactionAudio; // Audio to play on interaction
    public AudioClip secondaryAudio; // Audio to play after interaction

    [Header("Animation")]
    public GameObject secondaryObject; // Object with the animation
    public string secondaryAnimationName; // Name of the animation to play

    private AudioSource audioSource;
    private bool isInteracting = false;
    private bool isInteractionLocked = false;
    private bool canCancelInteraction = false;
    private string originalText; // To store the original text content

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        rawImage.enabled = false;

        if (interactionText != null)
        {
            originalText = interactionText.text;
            interactionText.enabled = false;
        }
    }

    private void Update()
    {
        if (isInteracting && canCancelInteraction && Input.anyKeyDown)
        {
            EndInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isInteractionLocked && interactionText != null)
        {
            interactionText.text = "Press E to interact";
            interactionText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && interactionText != null)
        {
            interactionText.text = originalText;
            interactionText.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player && !isInteractionLocked && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(HandleInteraction());
        }
    }

    private IEnumerator HandleInteraction()
    {
        isInteractionLocked = true;

        if (interactionText != null)
        {
            interactionText.text = originalText;
            interactionText.enabled = false;
        }

        isInteracting = true;
        rawImage.enabled = true;

        if (interactionAudio != null)
        {
            audioSource.clip = interactionAudio;
            audioSource.Play();
        }

        TogglePlayerControl(false);

        yield return new WaitForSeconds(3f);

        canCancelInteraction = true;
    }

    private void EndInteraction()
    {
        isInteracting = false;
        rawImage.enabled = false;
        canCancelInteraction = false;

        TogglePlayerControl(true);

        if (secondaryObject != null)
        {
            Animator animator = secondaryObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(secondaryAnimationName);
            }
        }

        if (secondaryAudio != null)
        {
            audioSource.clip = secondaryAudio;
            audioSource.Play();
        }

        gameObject.SetActive(false); // Disable to prevent further interactions
    }

    private void TogglePlayerControl(bool enable)
    {
        if (playerMovementController != null)
        {
            playerMovementController.enabled = enable;
        }

        if (cameraController != null)
        {
            cameraController.enabled = enable;
        }

        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enable;
    }
}
