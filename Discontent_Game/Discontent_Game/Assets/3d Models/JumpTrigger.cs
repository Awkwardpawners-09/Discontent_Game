using UnityEngine;
using UnityEngine.SceneManagement;  // This ensures you have access to SceneManager
using UnityEngine.UI;
using System.Collections; // Added this for IEnumerator support

public class JumpscareTrigger : MonoBehaviour
{
    public AudioSource Scream;        // This should be assigned an AudioSource in the Inspector
    public GameObject ThePlayer;
    public GameObject JumpCam;
    public RawImage FadeImage;        // RawImage to handle the fade effect
    public GameObject Jumpscare;      // The parent object (Jumpscare) that must be enabled

    public float jumpscareDuration = 2.0f;
    public float fadeDuration = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger and Jumpscare is enabled
        if (other.gameObject == ThePlayer && Jumpscare.activeSelf)
        {
            Debug.Log("Jumpscare Triggered!");
            if (Scream != null)
            {
                Scream.Play(); // Play the scream sound
            }
            JumpCam.SetActive(true); // Enable the JumpCam
            ThePlayer.SetActive(false); // Disable the player object
            StartCoroutine(HandleJumpscare());
        }
    }

    IEnumerator HandleJumpscare()
    {
        yield return new WaitForSeconds(jumpscareDuration); // Wait for the duration of the jumpscare
        StartCoroutine(FadeAndReset());
    }

    IEnumerator FadeAndReset()
    {
        float elapsedTime = 0f;

        // Ensure the RawImage starts fully transparent
        SetFadeImageAlpha(0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Fade in effect
            SetFadeImageAlpha(newAlpha);
            yield return null;
        }

        // Reload the current scene after fade is complete
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetFadeImageAlpha(float alpha)
    {
        if (FadeImage != null)
        {
            Color color = FadeImage.color;
            color.a = alpha;
            FadeImage.color = color;
        }
    }

    // Make sure this object has a trigger collider
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("The collider on this object is not set to trigger.");
        }
    }
}
