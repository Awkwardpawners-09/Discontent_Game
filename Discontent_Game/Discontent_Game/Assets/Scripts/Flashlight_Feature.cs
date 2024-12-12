using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight_Feature : MonoBehaviour
{
    public Light flashlight; // Attach the spotlight here
    public GameObject linkedObject; // Attach the object to enable/disable with the flashlight
    public AudioClip flashlightToggleSound; // Add the toggle sound
    public AudioClip batteryDepletedSound; // Sound for battery depletion
    public AudioClip cooldownWarningSound; // Sound when trying to use flashlight during cooldown
    public float maxBattery = 100f; // Maximum battery level
    public float batteryDepletionRate = 1f; // Battery depletion per second when flashlight is on
    public float batteryRechargeRate = 5f; // Battery recharge per second when flashlight is off
    public float minIntensity = 0.1f; // Minimum flashlight intensity when the battery is nearly empty
    public float maxIntensity = 1f; // Maximum flashlight intensity when the battery is full
    public float flickerThreshold = 10f; // Battery level at which the flashlight starts flickering
    public float flickerInterval = 0.2f; // Time interval between flickers
    public float cooldownDuration = 20f; // Time in seconds before the flashlight can be used again

    private AudioSource audioSource;
    private float currentBattery;
    private bool flashlightOn;
    private bool isOnCooldown;
    private bool isFlickering;

    void Start()
    {
        if (flashlight == null)
        {
            Debug.LogError("Flashlight (Light) is not assigned!");
            return;
        }

        if (linkedObject != null)
        {
            linkedObject.SetActive(false); // Ensure the linked object is off initially
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        currentBattery = maxBattery;
        flashlight.intensity = maxIntensity;
        flashlight.enabled = false; // Flashlight is off by default
        flashlightOn = false;
        isOnCooldown = false;
        isFlickering = false;
    }

    void Update()
    {
        Debug.Log($"Battery: {currentBattery:F1}%"); // Debug log to show battery level

        HandleFlashlightToggle();
        UpdateBattery();
        UpdateFlashlightIntensity();
    }

    private void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOnCooldown)
            {
                PlayCooldownWarningSound();
            }
            else if (currentBattery > 0 || flashlightOn) // Allow turning off even when battery is low
            {
                flashlightOn = !flashlightOn;
                flashlight.enabled = flashlightOn;

                // Enable or disable the linked object
                if (linkedObject != null)
                {
                    linkedObject.SetActive(flashlightOn);
                }

                if (flashlightToggleSound != null)
                {
                    audioSource.clip = flashlightToggleSound;
                    audioSource.Play();
                }
            }
        }
    }

    private void UpdateBattery()
    {
        if (flashlightOn && currentBattery > 0)
        {
            currentBattery -= batteryDepletionRate * Time.deltaTime;
            currentBattery = Mathf.Max(currentBattery, 0); // Prevent negative battery

            if (currentBattery <= flickerThreshold && !isFlickering)
            {
                StartCoroutine(FlickerEffect());
            }

            if (currentBattery == 0 && !isOnCooldown)
            {
                StartCoroutine(BatteryCooldown());
            }
        }
        else if (!flashlightOn && !isOnCooldown && currentBattery < maxBattery)
        {
            currentBattery += batteryRechargeRate * Time.deltaTime;
            currentBattery = Mathf.Min(currentBattery, maxBattery); // Prevent exceeding max battery
        }
    }

    private void UpdateFlashlightIntensity()
    {
        if (flashlightOn)
        {
            // Gradually decrease intensity based on battery percentage
            float batteryPercentage = currentBattery / maxBattery;
            flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryPercentage);
        }
    }

    private IEnumerator FlickerEffect()
    {
        isFlickering = true;

        while (flashlightOn && currentBattery <= flickerThreshold && currentBattery > 0)
        {
            flashlight.enabled = !flashlight.enabled; // Toggle flashlight
            yield return new WaitForSeconds(flickerInterval);
        }

        flashlight.enabled = flashlightOn; // Ensure it's on if still toggled
        isFlickering = false;
    }

    private IEnumerator BatteryCooldown()
    {
        isOnCooldown = true;
        flashlight.enabled = false;
        flashlightOn = false;

        if (linkedObject != null)
        {
            linkedObject.SetActive(false); // Ensure linked object is off
        }

        if (batteryDepletedSound != null)
        {
            audioSource.clip = batteryDepletedSound;
            audioSource.Play();
        }

        Debug.Log("Battery depleted. Starting cooldown...");

        // Wait 1 second before starting the cooldown
        yield return new WaitForSeconds(1f);

        // Wait for the cooldown duration
        float cooldownTimer = cooldownDuration;
        while (cooldownTimer > 0)
        {
            Debug.Log($"Cooldown: {cooldownTimer:F1}s remaining.");
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        currentBattery = maxBattery;
        isOnCooldown = false;

        Debug.Log("Flashlight recharged to 100%. Ready for use.");
    }

    private void PlayCooldownWarningSound()
    {
        if (cooldownWarningSound != null)
        {
            audioSource.clip = cooldownWarningSound;
            audioSource.Play();
        }

        Debug.Log("Flashlight is on cooldown!");
    }
}
