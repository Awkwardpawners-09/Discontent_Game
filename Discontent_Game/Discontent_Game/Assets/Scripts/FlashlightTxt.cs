using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteractionText : MonoBehaviour
{
    public Text targetText;             // Drag and drop the Text UI element
    public GameObject interactableObject; // The object to interact with (e.g., flashlight)
    public float displayDuration = 3.0f; // Duration the text remains visible before fading
    public float fadeDuration = 2.0f;   // Duration for the fade-out effect

    private Color originalColor;

    void Start()
    {
        originalColor = targetText.color; // Store the original color of the text
        targetText.gameObject.SetActive(false); // Ensure text is initially disabled
    }

    public void TriggerInteraction()
    {
        // Enable the text and start the sequence
        targetText.gameObject.SetActive(true);
        targetText.color = originalColor; // Reset color in case of multiple triggers
        StartCoroutine(HandleTextDisplay());
    }

    IEnumerator HandleTextDisplay()
    {
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Start fading the text
        yield return StartCoroutine(FadeText());
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == interactableObject)
        {
            TriggerInteraction(); // Trigger the interaction when the player touches the object
            interactableObject.SetActive(false); // Disable the object (optional)
        }
    }
}
