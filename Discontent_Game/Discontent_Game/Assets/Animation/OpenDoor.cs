using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject Instruction; // UI prompt for interaction
    public GameObject AnimeObject; // The door object with an Animator
    public GameObject ThisTrigger; // The trigger object to disable after interaction
    public AudioClip DoorOpenSound; // Sound for opening an unlocked door
    public AudioClip DoorLockedSound; // Sound for trying to open a locked door
    public bool Action = false; // Tracks if the player is near the door
    public bool IsLocked = false; // Toggle this in the Inspector to lock/unlock the door
    public string UnlockedAnimationName = "DoorOpen"; // Animation to play when the door is unlocked
    public string LockedAnimationName = "DoorLocked"; // Animation to play when the door is locked

    void Start()
    {
        Instruction.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Instruction.SetActive(false);
            Action = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Action)
        {
            Instruction.SetActive(false);

            if (IsLocked)
            {
                HandleLockedDoor();
            }
            else
            {
                HandleUnlockedDoor();
            }

            Action = false;
        }
    }

    void HandleUnlockedDoor()
    {
        // Play the unlocked door animation
        if (AnimeObject != null)
        {
            AnimeObject.GetComponent<Animator>().Play(UnlockedAnimationName);
        }

        // Play the unlocked door sound
        PlaySound(DoorOpenSound);

        // Disable the trigger to prevent repeated interaction
        if (ThisTrigger != null)
        {
            ThisTrigger.SetActive(false);
        }
    }

    void HandleLockedDoor()
    {
        // Play the locked door animation
        if (AnimeObject != null)
        {
            AnimeObject.GetComponent<Animator>().Play(LockedAnimationName);
        }

        // Play the locked door sound
        PlaySound(DoorLockedSound);

        Debug.Log("The door is locked!");
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned!");
        }
    }
}
