using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Trigger : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player; // Assign the player object in the Inspector
    public Transform cameraTransform; // Assign the player's camera for mouse locking

    [Header("UI Elements")]
    public Text interactionText; // Text to display "Press E to interact"
    public RawImage rawImageObject; // UI Raw Image to display

    [Header("Audio Settings")]
    public AudioClip interactionAudio; // Audio to play when interacting
    public AudioClip secondaryAudio; // Secondary audio to play after interaction

    [Header("Objects & Animation")]
    public GameObject secondaryObject; // Object to play the animation
    public string secondaryAnimationName; // Animation to play on the secondary object

    private AudioSource audioSource;
    private bool isInteracting = false;
    private bool canReactivateKeys = false;

    private void Start()
    {
        if (interactionText != null)
            interactionText.enabled = false;

        if (rawImageObject != null)
            rawImageObject.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isInteracting)
        {
            if (interactionText != null)
                interactionText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && !isInteracting)
        {
            if (interactionText != null)
                interactionText.enabled = false;
        }
    }

    private void Update()
    {
        if (isInteracting)
        {
            if (canReactivateKeys && Input.anyKeyDown)
            {
                EndInteraction();
            }
            return;
        }

        if (interactionText != null && interactionText.enabled && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(HandleInteraction());
        }
    }

    private IEnumerator HandleInteraction()
    {
        isInteracting = true;

        if (interactionText != null)
            interactionText.enabled = false;

        if (rawImageObject != null)
            rawImageObject.enabled = true;

        if (interactionAudio != null)
        {
            audioSource.clip = interactionAudio;
            audioSource.Play();
        }

        LockMouse(true);

        yield return new WaitForSeconds(3f);
        canReactivateKeys = true;
    }

    private void EndInteraction()
    {
        if (rawImageObject != null)
            rawImageObject.enabled = false;

        LockMouse(false);

        canReactivateKeys = false;
        isInteracting = false;

        if (secondaryObject != null)
        {
            Animator animator = secondaryObject.GetComponent<Animator>();
            if (animator != null && !string.IsNullOrEmpty(secondaryAnimationName))
                animator.Play(secondaryAnimationName);

            if (secondaryAudio != null)
            {
                audioSource.clip = secondaryAudio;
                audioSource.Play();
            }
        }

        gameObject.SetActive(false); // Disable this object
    }

    private void LockMouse(bool lockState)
    {
        if (lockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
