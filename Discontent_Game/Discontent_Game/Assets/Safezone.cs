using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safezone : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject playerObject; // Assign the player object in the Inspector

    private BreathingMechanic breathingMechanic; // Reference to the BreathingMechanic script

    void Start()
    {
        // Automatically find the BreathingMechanic script in the scene
        breathingMechanic = FindObjectOfType<BreathingMechanic>();

        if (breathingMechanic == null)
        {
            Debug.LogError("BreathingMechanic script not found in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject && breathingMechanic != null)
        {
            breathingMechanic.EnterSafeZone();
            Debug.Log("Player entered a safe zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObject && breathingMechanic != null)
        {
            breathingMechanic.ExitSafeZone();
            Debug.Log("Player exited the safe zone.");
        }
    }
}
