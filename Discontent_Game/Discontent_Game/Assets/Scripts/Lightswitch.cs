using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightswitch : MonoBehaviour
{
    public Text interactionText; // Assign the UI Text in the Inspector
    public GameObject lightObject; // Assign the light object to toggle
    public AudioClip toggleSound; // Assign the sound to play when toggling
    public float toggleCooldown = 1f; // Cooldown time in seconds

    private AudioSource audioSource;
    private bool isPlayerNearby = false;
    private bool canToggle = true; // Tracks if the light can be toggled

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide interaction text at the start
        }

        if (lightObject == null)
        {
            Debug.LogError("Light object is not assigned!");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure it doesn't play on start
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true); // Show interaction text
                interactionText.text = "[E]";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Hide interaction text
            }
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && canToggle)
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        if (lightObject != null)
        {
            lightObject.SetActive(!lightObject.activeSelf); // Toggle the light on/off
        }

        if (toggleSound != null)
        {
            audioSource.clip = toggleSound;
            audioSource.Play(); // Play the toggle sound
        }

        StartCoroutine(ToggleCooldown());
    }

    private System.Collections.IEnumerator ToggleCooldown()
    {
        canToggle = false; // Disable toggling
        yield return new WaitForSeconds(toggleCooldown); // Wait for cooldown
        canToggle = true; // Re-enable toggling
    }
}
