using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingMechanic : MonoBehaviour
{
    [Header("Flashlight Reference")]
    public Light flashlight; // Assign the flashlight Light component in the Inspector
    public AudioClip normalBreathingSound; // Audio for normal breathing
    public AudioClip heavyBreathingSound; // Audio for heavy breathing
    public AudioClip hauntingSound; // Audio for the haunting event

    [Header("Breathing Timers")]
    public float heavyBreathingDelay = 8f; // Time in seconds before heavy breathing starts
    public float hauntingTriggerDelay = 12f; // Time in seconds before haunting starts after heavy breathing
    public float hauntingDuration = 30f; // Duration of haunting sound in seconds

    [Header("Safe Zones")]
    public List<GameObject> safeZoneObjects; // Assign multiple safe zone objects in the Inspector

    private AudioSource breathingAudioSource;
    private AudioSource hauntingAudioSource;
    private bool isFlashlightOn = true;
    private bool isCurrentlyHeavyBreathing = false;
    private float heavyBreathingTimer = 0f; // Tracks time spent in heavy breathing
    private HashSet<GameObject> currentlyCollidingSafeZones = new HashSet<GameObject>(); // Keep track of colliding safe zones

    void Start()
    {
        // Initialize the breathing audio source
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
        breathingAudioSource.loop = true;
        breathingAudioSource.playOnAwake = false;

        // Initialize the haunting audio source
        hauntingAudioSource = gameObject.AddComponent<AudioSource>();
        hauntingAudioSource.loop = false;
        hauntingAudioSource.playOnAwake = false;

        if (flashlight == null)
        {
            Debug.LogError("Flashlight Light component not assigned!");
        }

        // Start normal breathing by default
        StartNormalBreathing();
    }

    void Update()
    {
        // Update flashlight state
        if (flashlight != null)
        {
            isFlashlightOn = flashlight.enabled;
        }

        // Check if the player is "safe"
        bool isPlayerSafe = isFlashlightOn || currentlyCollidingSafeZones.Count > 0;

        // Debug log to see the current state
        Debug.Log($"Is Player Safe: {isPlayerSafe}, Flashlight On: {isFlashlightOn}, Safe Zones Count: {currentlyCollidingSafeZones.Count}");

        if (isPlayerSafe)
        {
            if (isCurrentlyHeavyBreathing)
            {
                StartNormalBreathing();
            }
            heavyBreathingTimer = 0f; // Reset heavy breathing timer
        }
        else
        {
            // Increment heavy breathing timer if not "safe"
            heavyBreathingTimer += Time.deltaTime;

            if (!isCurrentlyHeavyBreathing && heavyBreathingTimer >= heavyBreathingDelay)
            {
                StartHeavyBreathing();
            }

            // Check if heavy breathing has lasted long enough to trigger haunting
            if (isCurrentlyHeavyBreathing && heavyBreathingTimer >= hauntingTriggerDelay)
            {
                TriggerHaunting();
                heavyBreathingTimer = 0f; // Reset the timer after triggering haunting
            }
        }
    }

    private void StartHeavyBreathing()
    {
        if (heavyBreathingSound != null && breathingAudioSource.clip != heavyBreathingSound)
        {
            breathingAudioSource.clip = heavyBreathingSound;
            breathingAudioSource.volume = 1f; // Adjust volume as needed
            breathingAudioSource.Play();
            isCurrentlyHeavyBreathing = true;
            Debug.Log("Heavy Breathing started.");
        }
    }

    private void StartNormalBreathing()
    {
        if (normalBreathingSound != null && breathingAudioSource.clip != normalBreathingSound)
        {
            breathingAudioSource.clip = normalBreathingSound;
            breathingAudioSource.volume = 0.7f; // Adjust volume as needed
            breathingAudioSource.Play();
            isCurrentlyHeavyBreathing = false;
            Debug.Log("Normal Breathing started.");
        }
    }

    private void TriggerHaunting()
    {
        if (hauntingSound != null)
        {
            hauntingAudioSource.clip = hauntingSound;
            hauntingAudioSource.volume = 1f; // Adjust volume as needed
            hauntingAudioSource.Play();
            Debug.Log("Haunting triggered!");

            StartCoroutine(StopHauntingAfterDuration());
        }
    }

    private IEnumerator StopHauntingAfterDuration()
    {
        yield return new WaitForSeconds(hauntingDuration);
        hauntingAudioSource.Stop();
        Debug.Log("Haunting ended.");
    }

    void OnTriggerEnter(Collider other)
    {
        // Add to the set if the player collides with a safe zone
        if (safeZoneObjects.Contains(other.gameObject))
        {
            currentlyCollidingSafeZones.Add(other.gameObject);
            Debug.Log($"Player entered a safe zone: {other.gameObject.name}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Remove from the set if the player exits a safe zone
        if (currentlyCollidingSafeZones.Contains(other.gameObject))
        {
            currentlyCollidingSafeZones.Remove(other.gameObject);
            Debug.Log($"Player exited a safe zone: {other.gameObject.name}");
        }
    }
}
