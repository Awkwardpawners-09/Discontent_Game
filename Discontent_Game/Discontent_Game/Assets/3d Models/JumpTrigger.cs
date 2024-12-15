using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpTrigger : MonoBehaviour
{
    public AudioSource Scream;
    public GameObject ThePlayer;
    public GameObject JumpCam;
    public CanvasGroup FadeCanvasGroup;

    public float jumpscareDuration = 2.0f;
    public float fadeDuration = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ThePlayer)
        {
            Scream.Play();
            JumpCam.SetActive(true);
            ThePlayer.SetActive(false);
            StartCoroutine(HandleJumpscare());
        }
    }

    IEnumerator HandleJumpscare()
    {
        yield return new WaitForSeconds(jumpscareDuration);
        StartCoroutine(FadeAndReset());
    }

    IEnumerator FadeAndReset()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            FadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
