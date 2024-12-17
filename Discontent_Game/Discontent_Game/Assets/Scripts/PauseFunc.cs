using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject PauseOverlay;         // The UI overlay or menu
    public GameObject Dimm;                 // The Dimm panel (not the CanvasGroup itself)
    public CanvasGroup GameDimmerCanvasGroup; // CanvasGroup for the background dimmer
    public GameObject PausedText;           // The "Game is PAUSED" text or image
    public float dimOpacity = 0.5f;         // The opacity level for dimming (0 to 1)
    public float fadeDuration = 0.5f;       // The duration of the fade effect in seconds

    private bool isPaused = false;          // Tracks whether the game is paused

    void Start()
    {
        // Ensure that the Dimm panel is initially disabled
        Dimm.SetActive(false);
    }

    void Update()
    {
        // Toggle pause when Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game

        // Activate Dimm panel when game is paused
        Dimm.SetActive(true);

        // Start fading in the dimmer effect
        StartCoroutine(FadeInDimmer());

        PauseOverlay.SetActive(true);   // Activate the pause menu overlay
        PausedText.SetActive(true);     // Show the "Game is PAUSED" text
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game

        // Start fading out the dimmer effect
        StartCoroutine(FadeOutDimmer());

        PauseOverlay.SetActive(false);   // Deactivate the pause menu overlay
        PausedText.SetActive(false);     // Hide the "Game is PAUSED" text

        // Deactivate Dimm panel when resuming
        Dimm.SetActive(false);
    }

    private IEnumerator FadeInDimmer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time for pauses
            GameDimmerCanvasGroup.alpha = Mathf.Lerp(0f, dimOpacity, elapsedTime / fadeDuration);
            yield return null;
        }
        GameDimmerCanvasGroup.alpha = dimOpacity;
    }

    private IEnumerator FadeOutDimmer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time for pauses
            GameDimmerCanvasGroup.alpha = Mathf.Lerp(dimOpacity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        GameDimmerCanvasGroup.alpha = 0f;
    }
}
