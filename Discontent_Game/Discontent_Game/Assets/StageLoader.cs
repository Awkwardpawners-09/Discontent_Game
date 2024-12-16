using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject playerObject; // Assign the player object in the Inspector

    [Header("Stage Management")]
    public GameObject previousStage; // The stage to deactivate
    public GameObject nextStage;     // The stage to activate

    private bool hasTriggered = false; // Ensures the transition happens only once

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject == playerObject)
        {
            hasTriggered = true; // Prevent retriggering
            HandleStageTransition();
        }
    }

    private void HandleStageTransition()
    {
        // Deactivate the previous stage objects
        if (previousStage != null)
        {
            previousStage.SetActive(false);
        }

        // Activate the next stage objects
        if (nextStage != null)
        {
            nextStage.SetActive(true);
        }

        Debug.Log("Stage transition completed.");
    }
}
