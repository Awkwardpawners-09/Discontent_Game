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
    public float normalBreathingDelay = 5f; // Time in seconds before normal breathing starts
    public float hauntingTriggerDelay = 12f; // Time in seconds before haunting starts after heavy breathing
    public float hauntingDuration = 30f; // Duration of haunting sound in seconds

    [Header("Player State")]
    public GameObject playerObject; // The object to consider as the player
    public bool isInDanger = false; // Toggle to indicate if the player is in danger

    private AudioSource breathingAudioSource;
    private AudioSource hauntingAudioSource;
    private float flashlightOffTimer = 0f;
    private float flashlightOnTimer = 0f;
    private bool isFlashlightOn = true;
    private bool isCurrentlyHeavyBreathing = false;
    private float heavyBreathingTimer = 0f; // Tracks time spent in heavy breathing
    private bool isInLight = false; // Tracks if the player is in a "light" area

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

        if (playerObject == null)
        {
            Debug.LogError("Player Object not assigned!");
        }
    }

    void Update()
    {
        // Handle player breathing based on danger state, flashlight, and light zones
        if (isInDanger)
        {
            StartHeavyBreathing(); // Override flashlight logic
            ResetTimers();
        }
        else
        {
            if (flashlight != null)
            {
                isFlashlightOn = flashlight.enabled;

                if (isFlashlightOn)
                {
                    HandleFlashlightOn();
                }
                else
                {
                    HandleFlashlightOff();
                }
            }

            // Prevent heavy breathing if the player is in a "light" zone
            if (isInLight && isCurrentlyHeavyBreathing)
            {
                StartNormalBreathing();
            }
        }

        // Check if heavy breathing has lasted long enough to trigger haunting
        if (isCurrentlyHeavyBreathing && !isInLight)
        {
            heavyBreathingTimer += Time.deltaTime;

            if (heavyBreathingTimer >= hauntingTriggerDelay)
            {
                TriggerHaunting();
                heavyBreathingTimer = 0f; // Reset the timer after triggering haunting
            }
        }
        else
        {
            heavyBreathingTimer = 0f; // Reset timer if heavy breathing stops or player is in light
        }
    }

    private void HandleFlashlightOff()
    {
        flashlightOffTimer += Time.deltaTime;
        flashlightOnTimer = 0f; // Reset on-timer since flashlight is off

        if (flashlightOffTimer >= heavyBreathingDelay && !isCurrentlyHeavyBreathing && !isInLight)
        {
            StartHeavyBreathing();
        }
    }

    private void HandleFlashlightOn()
    {
        flashlightOnTimer += Time.deltaTime;
        flashlightOffTimer = 0f; // Reset off-timer since flashlight is on

        if (flashlightOnTimer >= normalBreathingDelay && isCurrentlyHeavyBreathing)
        {
            StartNormalBreathing();
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

    private System.Collections.IEnumerator StopHauntingAfterDuration()
    {
        yield return new WaitForSeconds(hauntingDuration);
        hauntingAudioSource.Stop();
        Debug.Log("Haunting ended.");
    }

    private void ResetTimers()
    {
        flashlightOffTimer = 0f;
        flashlightOnTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light") && other.gameObject == playerObject)
        {
            isInLight = true;
            Debug.Log("Player entered a light zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light") && other.gameObject == playerObject)
        {
            isInLight = false;
            Debug.Log("Player exited the light zone.");
        }
    }
}
