using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingSequence : MonoBehaviour
{
    [Header("References")]
    public RawImage endingImage; // The RawImage to fade
    public GameObject currentPlayerObject; // The current player object to disable
    public GameObject newPlayerObject; // The new player object to enable
    public AudioSource currentAudio; // The current audio source to disable
    public AudioSource newAudio; // The new audio source to enable

    [Header("Durations")]
    public float fadeDuration = 2f; // Time to fade in/out the RawImage
    public float animationDuration = 29f; // Duration of the "Endanimate" animation

    [Header("Animation Settings")]
    public string animationName = "Endanimate"; // The name of the animation to play on the new player object

    private Animator newPlayerAnimator;
    private bool isFading = false;

    private void Start()
    {
        if (endingImage == null)
        {
            Debug.LogError("Ending Image is not assigned!");
            return;
        }

        if (newPlayerObject != null)
        {
            newPlayerAnimator = newPlayerObject.GetComponent<Animator>();
            if (newPlayerAnimator == null)
            {
                Debug.LogError("No Animator found on the new player object!");
            }
        }
        else
        {
            Debug.LogError("New Player Object is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with this object
        if (other.gameObject == currentPlayerObject)
        {
            StartCoroutine(PlayEndingSequence());
        }
    }

    private IEnumerator PlayEndingSequence()
    {
        // Step 1: Fade the RawImage from 0 to 100 transparency
        yield return FadeRawImage(endingImage, 0f, 1f, fadeDuration);

        // Step 2: Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Step 3: Fade the RawImage from 100 to 0 transparency while the animation plays
        if (currentPlayerObject != null)
        {
            currentPlayerObject.SetActive(false); // Disable the current player object
        }

        if (newPlayerObject != null)
        {
            newPlayerObject.SetActive(true); // Enable the new player object

            // Play the animation on the new player object
            if (newPlayerAnimator != null)
            {
                newPlayerAnimator.Play(animationName);
            }
        }

        // Swap the audio
        if (currentAudio != null)
        {
            currentAudio.Stop();
            currentAudio.enabled = false;
        }

        if (newAudio != null)
        {
            newAudio.enabled = true;
            newAudio.Play();
        }

        // Fade the RawImage from 100 to 0 transparency
        yield return FadeRawImage(endingImage, 1f, 0f, fadeDuration);

        // Step 4: Wait for 27 seconds
        yield return new WaitForSeconds(27f);

        // Step 5: Fade the RawImage from 0 to 100 transparency
        yield return FadeRawImage(endingImage, 0f, 1f, fadeDuration);

        // Ensure the RawImage is fully opaque
        endingImage.color = new Color(endingImage.color.r, endingImage.color.g, endingImage.color.b, 1);
    }

    private IEnumerator FadeRawImage(RawImage image, float startAlpha, float endAlpha, float duration)
    {
        if (image != null)
        {
            isFading = true;

            Color initialColor = image.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                image.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                yield return null;
            }

            isFading = false;
        }
    }
}
