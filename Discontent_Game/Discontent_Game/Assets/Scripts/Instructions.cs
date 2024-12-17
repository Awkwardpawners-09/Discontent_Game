using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnableAndFadeText : MonoBehaviour
{
    public Animation cameraAnimation;    // Drag and drop the Animation component
    public AnimationClip animationClip;  // Drag and drop the animation clip
    public Text targetText;              // Drag and drop the Text UI element
    public Button pauseButton;           // Drag and drop the Pause Button UI element
    public float delay = 2.0f;           // Delay before enabling the text
    public float fadeDuration = 2.0f;    // Duration for the fade-out effect
    public float displayDuration = 3.0f; // Duration the text remains visible before fading

    private Color originalColor;

    void Start()
    {
        originalColor = targetText.color; // Store the original color of the text
        targetText.gameObject.SetActive(false); // Ensure text is initially disabled
        pauseButton.gameObject.SetActive(false); // Ensure pause button is initially hidden
        StartCoroutine(HandleTextAndPauseButtonDisplay());
    }

    IEnumerator HandleTextAndPauseButtonDisplay()
    {
        // Wait until the animation is complete
        yield return new WaitForSeconds(animationClip.length);

        // Add optional delay after animation
        yield return new WaitForSeconds(delay);

        // Enable the text
        targetText.gameObject.SetActive(true);

        // Wait for the text to remain visible
        yield return new WaitForSeconds(displayDuration);

        // Start fading the text
        yield return StartCoroutine(FadeText());

        // Enable the pause button after the animation and text fade
        pauseButton.gameObject.SetActive(true);
    }

    IEnumerator FadeText()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Disable the text completely after fading
        targetText.gameObject.SetActive(false);
    }
}
