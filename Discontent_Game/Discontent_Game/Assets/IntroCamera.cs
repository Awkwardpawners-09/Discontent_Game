using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCamera : MonoBehaviour
{
    [Header("References")]
    public RawImage introImage; // The RawImage to fade
    public GameObject animationObject; // The player object with an Animator component
    public GameObject cameraObject; // The object with the CameraController

    [Header("Durations")]
    public float fadeDuration = 2f; // Time to fade out the RawImage
    public float disableDuration = 18f; // Total time player is restricted from control

    [Header("Animation Settings")]
    public string animationName = "Idle"; // The name of the animation to play on the Animator

    private bool isFading = false;
    private MovementController movementController;
    private CameraController cameraController;
    private Animator playerAnimator;

    private void Start()
    {
        if (introImage == null)
        {
            Debug.LogError("Intro Image is not assigned!");
            return;
        }

        // Cache the MovementController from the player object
        movementController = animationObject.transform.root.GetComponent<MovementController>();

        if (movementController == null)
        {
            Debug.LogError("No MovementController found on the player object!");
        }

        // Cache the CameraController from the cameraObject
        if (cameraObject != null)
        {
            cameraController = cameraObject.GetComponent<CameraController>();
            if (cameraController == null)
            {
                Debug.LogError("No CameraController found on the camera object!");
            }
        }
        else
        {
            Debug.LogError("CameraObject is not assigned!");
        }

        // Cache the Animator (Playeranimation) from the player object
        playerAnimator = animationObject.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("No Animator (Playeranimation) found on the animation object!");
        }

        // Start the intro sequence
        StartCoroutine(StageIntroSequence());
    }

    private IEnumerator StageIntroSequence()
    {
        // Disable player movement and mouse control
        RestrictPlayerControl(true);

        // Disable camera control
        if (cameraController != null)
        {
            cameraController.enabled = false; // Disable CameraController
        }

        // Trigger the animation
        if (animationObject != null && playerAnimator != null)
        {
            playerAnimator.Play(animationName); // Play the animation by name
        }

        // Fade out the RawImage
        yield return FadeOutRawImage(introImage, fadeDuration);

        // Wait for the remaining disable time (subtract the fade duration)
        yield return new WaitForSeconds(disableDuration - fadeDuration);

        // Re-enable player controls
        RestrictPlayerControl(false);

        // Re-enable camera control
        if (cameraController != null)
        {
            cameraController.enabled = true; // Enable CameraController
        }

        // Disable the Animator after the animation has played (exactly at 18 seconds)
        if (playerAnimator != null)
        {
            playerAnimator.enabled = false; // Disable the Animator after 18 seconds
        }

        // Ensure the RawImage is fully transparent and inactive
        introImage.color = new Color(introImage.color.r, introImage.color.g, introImage.color.b, 0);
        introImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutRawImage(RawImage image, float duration)
    {
        if (image != null)
        {
            isFading = true;

            Color initialColor = image.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                image.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                yield return null;
            }

            isFading = false;
        }
    }

    private void RestrictPlayerControl(bool restrict)
    {
        if (movementController != null)
        {
            movementController.enabled = !restrict; // Disable or enable the MovementController
        }

        // Lock or unlock the cursor
        Cursor.lockState = restrict ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !restrict;
    }
}
