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
    public float safeZoneRecoveryTime = 6f; // Time required to recover in a safe zone
    public float hauntingTriggerDelay = 12f; // Time in seconds before haunting starts after heavy breathing
    public float hauntingDuration = 30f; // Duration of haunting sound in seconds

    private AudioSource breathingAudioSource;
    private AudioSource hauntingAudioSource;
    private bool isCurrentlyHeavyBreathing = false;
    private float heavyBreathingTimer = 0f; // Tracks time spent in heavy breathing
    private float recoveryTimer = 0f; // Tracks continuous recovery in safe zones
    private int safeZoneCount = 0; // Tracks the number of safe zones the player is inside

    void Start()
    {
        // Initialize audio sources
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
        hauntingAudioSource = gameObject.AddComponent<AudioSource>();

        breathingAudioSource.loop = true;
        hauntingAudioSource.loop = false;

        StartNormalBreathing();
    }

    void Update()
    {
        bool isPlayerSafe = flashlight.enabled || safeZoneCount > 0;

        if (isPlayerSafe)
        {
            if (isCurrentlyHeavyBreathing)
            {
                recoveryTimer += Time.deltaTime;
                if (recoveryTimer >= safeZoneRecoveryTime)
                {
                    StartNormalBreathing();
                }
            }
            else
            {
                heavyBreathingTimer = 0f; // Reset heavy breathing timer
            }
        }
        else
        {
            recoveryTimer = 0f; // Reset recovery timer
            heavyBreathingTimer += Time.deltaTime;

            if (!isCurrentlyHeavyBreathing && heavyBreathingTimer >= heavyBreathingDelay)
            {
                StartHeavyBreathing();
            }

            if (isCurrentlyHeavyBreathing && heavyBreathingTimer >= hauntingTriggerDelay)
            {
                TriggerHaunting();
                heavyBreathingTimer = 0f; // Reset after haunting
            }
        }
    }

    private void StartHeavyBreathing()
    {
        if (heavyBreathingSound != null)
        {
            breathingAudioSource.clip = heavyBreathingSound;
            breathingAudioSource.Play();
            isCurrentlyHeavyBreathing = true;
            Debug.Log("Heavy Breathing started.");
        }
    }

    private void StartNormalBreathing()
    {
        if (normalBreathingSound != null)
        {
            breathingAudioSource.clip = normalBreathingSound;
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
            hauntingAudioSource.Play();
            StartCoroutine(StopHauntingAfterDuration());
            Debug.Log("Haunting triggered.");
        }
    }

    private System.Collections.IEnumerator StopHauntingAfterDuration()
    {
        yield return new WaitForSeconds(hauntingDuration);
        hauntingAudioSource.Stop();
        Debug.Log("Haunting ended.");
    }

    public void EnterSafeZone()
    {
        safeZoneCount++;
    }

    public void ExitSafeZone()
    {
        safeZoneCount = Mathf.Max(0, safeZoneCount - 1); // Ensure the count doesn't go below zero
    }
}
