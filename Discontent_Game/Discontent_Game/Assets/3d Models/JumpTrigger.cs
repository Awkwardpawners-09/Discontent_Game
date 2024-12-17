using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscareTrigger : MonoBehaviour
{
    public AudioSource Scream;
    public GameObject JumpCam;
    public RawImage FadeImage;

    public float jumpscareDuration = 2.0f;
    public float fadeDuration = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jumpscare Triggered!");
            if (Scream != null)
            {
                Scream.Play();
            }
            JumpCam.SetActive(true);
            StartCoroutine(HandleJumpscare());
        }
    }

    IEnumerator HandleJumpscare()
    {
        yield return new WaitForSeconds(jumpscareDuration);
        StartCoroutine(FadeAndRestart());
    }

    IEnumerator FadeAndRestart()
    {
        float elapsedTime = 0f;

        // Fade-in effect
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetFadeImageAlpha(alpha);
            yield return null;
        }

        // Reset the game via GameManager
        GameManager.Instance.ResetGame();

        // Fade-out effect
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            SetFadeImageAlpha(alpha);
            yield return null;
        }

        JumpCam.SetActive(false); // Disable the jumpscare camera
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
}
