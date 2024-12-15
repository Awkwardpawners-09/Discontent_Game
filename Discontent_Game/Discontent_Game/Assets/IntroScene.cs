using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    [Header("Object References")]
    public GameObject firstObject; // Assign the first object in the Inspector
    public GameObject secondObject; // Assign the second object in the Inspector
    public GameObject thirdObject; // Assign the third object in the Inspector

    [Header("Scene Settings")]
    public string nextSceneName; // Name of the scene to load after the splash screens

    private void Start()
    {
        // Start the splash screen sequence
        StartCoroutine(SplashScreenSequence());
    }

    private IEnumerator SplashScreenSequence()
    {
        // Ensure all objects are initially inactive
        SetObjectActive(firstObject, false);
        SetObjectActive(secondObject, false);
        SetObjectActive(thirdObject, false);

        // First object sequence
        yield return new WaitForSeconds(3f); // Wait 3 seconds
        yield return FadeIn(firstObject, 1f); // Fade in over 1 second
        yield return new WaitForSeconds(3f); // Stay for 3 seconds
        yield return FadeOut(firstObject, 1f); // Fade out over 1 second

        // Second object sequence
        yield return new WaitForSeconds(7f); // Wait 7 seconds
        yield return FadeIn(secondObject, 5f); // Fade in over 5 seconds
        yield return FadeOut(secondObject, 1f); // Fade out over 1 second

        // Third object sequence
        yield return new WaitForSeconds(3f); // Wait 3 seconds
        SetObjectActive(thirdObject, true); // Instantly show the third object
        yield return new WaitForSeconds(5f); // Stay for 5 seconds
        yield return FadeOut(thirdObject, 2f); // Slowly fade out over 2 seconds

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private void SetObjectActive(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);
        }
    }

    private IEnumerator FadeIn(GameObject obj, float duration)
    {
        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsed = 0f;

                SetObjectActive(obj, true);

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Clamp01(elapsed / duration);
                    renderer.material.color = color;
                    yield return null;
                }
            }
        }
    }

    private IEnumerator FadeOut(GameObject obj, float duration)
    {
        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    color.a = 1f - Mathf.Clamp01(elapsed / duration);
                    renderer.material.color = color;
                    yield return null;
                }

                SetObjectActive(obj, false);
            }
        }
    }
}
