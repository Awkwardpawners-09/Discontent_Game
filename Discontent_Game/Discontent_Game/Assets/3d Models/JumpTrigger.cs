using System.Collections;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    public AudioSource Scream;           // The scream sound
    public GameObject ThePlayer;         // The player object
    public GameObject JumpCam;           // The jumpscare camera
    public GameObject FlashImg;          // The flash effect

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ThePlayer)
        {
            Scream.Play();
            JumpCam.SetActive(true);
            ThePlayer.SetActive(false);
            FlashImg.SetActive(true);
            StartCoroutine(EndJump());
        }
    }

    IEnumerator EndJump()
    {
        // Wait for the jumpscare duration
        yield return new WaitForSeconds(2.03f);

        // Notify GameManager to handle respawn or restart
        GameManager.Instance.HandleRespawn();
    }
}
