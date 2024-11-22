using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingMechanic : MonoBehaviour
{
    [Header("Flashlight Reference")]
    public Light flashlight; // Assign the flashlight Light component in the Inspector
    public AudioClip normalBreathingSound; // Audio for normal breathing
    public AudioClip heavyBreathingSound; // Audio for heavy breathing

    [Header("Breathing Timers")]
    public float heavyBreathingDelay = 8f; // Time in seconds before heavy breathing starts
    public float normalBreathingDelay = 5f; // Time in seconds before normal breathing starts

    [Header("Player State")]
    public bool isInDanger = false; // Toggle to indicate if the player is in danger

    private AudioSource breathingAudioSource;
    private float flashlightOffTimer = 0f;
    private float flashlightOnTimer = 0f;
    private bool isFlashlightOn = true;

    private bool isCurrentlyHeavyBreathing = false;

    void Start()
    {
        // Initialize the audio source
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
        breathingAudioSource.loop = true;
        breathingAudioSource.playOnAwake = false;

        if (flashlight == null)
        {
            Debug.LogError("Flashlight Light component not assigned!");
        }
    }

    void Update()
    {
        // Handle player breathing based on danger state and flashlight
        if (isInDanger)
        {
            StartHeavyBreathing(); // Override flashlight logic
            flashlightOffTimer = 0f; // Reset the flashlight-related timer
            flashlightOnTimer = 0f; // Reset the on-timer to avoid interference
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
        }

        Debug.Log($"Player Breathing: {(isCurrentlyHeavyBreathing ? "Heavy" : "Normal")} | Timer (Off): {flashlightOffTimer:F1} | Timer (On): {flashlightOnTimer:F1}");
    }

    private void HandleFlashlightOff()
    {
        flashlightOffTimer += Time.deltaTime;
        flashlightOnTimer = 0f; // Reset on-timer since flashlight is off

        if (flashlightOffTimer >= heavyBreathingDelay && !isCurrentlyHeavyBreathing)
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
}
