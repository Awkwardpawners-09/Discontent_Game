using System.Collections;
using UnityEngine;

public class BreathingMechanic : MonoBehaviour
{
    [Header("Player References")]
    public GameObject PlayerObject; // The player object to monitor movement
    public Transform PlayerCamera; // The camera to monitor rotation
    public GameObject JumpscareObject; // The jumpscare object to activate during a violation
    public AudioSource HauntingAudioSource; // The audio source to enable during the haunting

    [Header("Breathing Settings")]
    public AudioClip normalBreathingSound; // Audio for normal breathing
    public AudioClip heavyBreathingSound; // Audio for heavy breathing

    [Header("Breathing Volumes")]
    public float normalBreathingVolume = 0.7f; // Volume for normal breathing
    public float heavyBreathingVolume = 0.9f; // Volume for heavy breathing

    [Header("Breathing Timers")]
    public float heavyBreathingDelay = 8f; // Time in seconds before heavy breathing starts
    public float safeZoneRecoveryTime = 6f; // Time required to recover in a safe zone
    public float hauntingTriggerDelay = 12f; // Time in seconds before haunting starts after heavy breathing
    public float hauntingDuration = 30f; // Duration of haunting sound in seconds

    [Header("Movement and Camera Settings")]
    public float maxAllowedRotation = 180f; // Maximum allowed camera rotation (in degrees)
    public float allowedMovementDistance = 0.5f; // Maximum allowed player movement from initial position

    private AudioSource breathingAudioSource;
    private bool isCurrentlyHeavyBreathing = false;
    private float heavyBreathingTimer = 0f; // Tracks time spent in heavy breathing
    private float recoveryTimer = 0f; // Tracks continuous recovery in safe zones
    private int safeZoneCount = 0; // Tracks the number of safe zones the player is inside

    private Vector3 initialPlayerPosition;
    private Quaternion initialCameraRotation;
    private bool isHauntingActive = false;
    private bool jumpscareTriggered = false;

    [Header("Flashlight Reference")]
    public Light flashlight; // Add this reference for flashlight

    [Header("New Feature: Object During Haunting")]
    public GameObject hauntingObject; // Object to enable during haunting

    void Start()
    {
        // Initialize audio sources
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
        HauntingAudioSource.loop = false;

        breathingAudioSource.loop = true;

        StartNormalBreathing();

        // Ensure hauntingObject is initially disabled
        if (hauntingObject != null)
        {
            hauntingObject.SetActive(false);
        }
    }

    void Update()
    {
        bool isPlayerSafe = flashlight != null && flashlight.enabled || safeZoneCount > 0;

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

        if (isHauntingActive && !jumpscareTriggered)
        {
            // Check for movement and camera rotation violations during haunting
            CheckForViolations();
        }
    }

    private void StartHeavyBreathing()
    {
        if (heavyBreathingSound != null)
        {
            breathingAudioSource.clip = heavyBreathingSound;
            breathingAudioSource.volume = heavyBreathingVolume; // Adjust heavy breathing volume
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
            breathingAudioSource.volume = normalBreathingVolume; // Adjust normal breathing volume
            breathingAudioSource.Play();
            isCurrentlyHeavyBreathing = false;
            Debug.Log("Normal Breathing started.");
        }
    }

    private void TriggerHaunting()
    {
        isHauntingActive = true;

        if (HauntingAudioSource != null)
        {
            HauntingAudioSource.enabled = true; // Enable haunting audio
            HauntingAudioSource.Play();
        }

        if (hauntingObject != null)
        {
            hauntingObject.SetActive(true); // Enable the haunting object
        }

        initialPlayerPosition = PlayerObject.transform.position;
        initialCameraRotation = PlayerCamera.rotation;

        StartCoroutine(StopHauntingAfterDuration());
        Debug.Log("Haunting triggered.");
    }

    private void CheckForViolations()
    {
        // Check if the player moved beyond the allowed distance
        float distanceMoved = Vector3.Distance(initialPlayerPosition, PlayerObject.transform.position);
        if (distanceMoved > allowedMovementDistance)
        {
            TriggerJumpscare("Player moved too far.");
            return;
        }

        // Check if the camera rotated beyond the allowed angle
        float rotationDifference = Quaternion.Angle(initialCameraRotation, PlayerCamera.rotation);
        if (rotationDifference > maxAllowedRotation)
        {
            TriggerJumpscare("Player rotated the camera too far.");
        }
    }

    private void TriggerJumpscare(string reason)
    {
        jumpscareTriggered = true;

        if (JumpscareObject != null)
        {
            JumpscareObject.SetActive(true);
        }

        Debug.Log($"Jumpscare triggered: {reason}");
        EndHaunting();
    }

    private IEnumerator StopHauntingAfterDuration()
    {
        yield return new WaitForSeconds(hauntingDuration);
        EndHaunting();
        Debug.Log("Haunting ended.");
    }

    private void EndHaunting()
    {
        isHauntingActive = false;

        if (HauntingAudioSource != null)
        {
            HauntingAudioSource.enabled = false; // Disable haunting audio
        }

        if (hauntingObject != null)
        {
            hauntingObject.SetActive(false); // Disable the haunting object
        }
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
