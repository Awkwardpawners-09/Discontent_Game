using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscareTrigger : MonoBehaviour
{
    public AudioSource Scream;              // Scream sound
    public GameObject ThePlayer;            // Player object
    public GameObject JumpCam;              // Camera for jumpscare
    public RawImage FadeImage;              // RawImage for fade effect
    public GameObject Jumpscare;            // Parent jumpscare object
    public float jumpscareDuration = 2.0f;  // Duration of the jumpscare
    public float fadeDuration = 2.0f;       // Duration for fade effect

    // Player respawn position
    private Vector3 respawnPosition;

    private void Start()
    {
        // Set the initial checkpoint (starting position)
        respawnPosition = ThePlayer.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ThePlayer && Jumpscare.activeSelf)
        {
            Debug.Log("Jumpscare Triggered!");

            if (Scream != null)
            {
                Scream.Play();
            }

            JumpCam.SetActive(true);
            ThePlayer.SetActive(false); // "Disable" the player for jumpscare effect

            // Disable this collider so jumpscare doesn't trigger again
            GetComponent<Collider>().enabled = false;

            StartCoroutine(HandleJumpscare());
        }
    }

    IEnumerator HandleJumpscare()
    {
        yield return new WaitForSeconds(jumpscareDuration);
        StartCoroutine(FadeAndRespawn());
    }

    IEnumerator FadeAndRespawn()
    {
        float elapsedTime = 0f;

        // Fade-in effect
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetFadeImageAlpha(alpha);
            yield return null;
        }

        RespawnPlayer(); // Respawn the player at the latest checkpoint

        // Fade-out effect
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            SetFadeImageAlpha(alpha);
            yield return null;
        }
    }

    void RespawnPlayer()
    {
        Debug.Log("Respawning player at the latest checkpoint!");
        ThePlayer.transform.position = respawnPosition; // Move player to checkpoint
        ThePlayer.SetActive(true);                     // Reactivate the player
        JumpCam.SetActive(false);                      // Disable jumpscare camera
    }

    void SetFadeImageAlpha(float alpha)
    {
        if (FadeImage != null)
        {
            Color color = FadeImage.color;
            color.a = alpha;
            FadeImage.color = color;
        }
    }

    // Call this from a Checkpoint object when the player reaches it
    public void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
        Debug.Log("Checkpoint updated to position: " + checkpointPosition);
    }
}
