using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button startButton;           // The button to click
    public AudioClip clickSound;        // Sound to play on click (optional)
    public CanvasGroup canvasGroup;     // CanvasGroup for fade effect
    public string nextSceneName;        // Scene name (drag the scene here)
    public float fadeDuration = 1f;     // Duration of the fade in seconds

    private bool isFading = false;
    private AudioSource audioSource;    // Internal AudioSource for playing the sound

    void Start()
    {
        // Set up the AudioSource if it doesn't exist
        audioSource = gameObject.AddComponent<AudioSource>();

        // Ensure the button is set up correctly
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
        else
        {
            Debug.LogError("Start Button is not assigned.");
        }

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not assigned. Add a CanvasGroup component to your Canvas.");
        }
    }

    void OnStartButtonClick()
    {
        if (!isFading)
        {
            isFading = true;

            // Play the button click sound (if one is assigned)
            if (clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
            else
            {
                Debug.LogWarning("No click sound assigned!");
            }

            // Start the fade effect
            StartCoroutine(FadeAndLoadScene());
        }
    }

    System.Collections.IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        // Gradually reduce the alpha of the CanvasGroup to 0
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        // Ensure alpha is fully set to 0
        canvasGroup.alpha = 0;

        // Load the next scene (using the scene name as a string)
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not assigned!");
        }
    }
}
