using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagemanager : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player; // The player object to check for collisions

    [Header("Stage Settings")]
    public List<GameObject> stagePrefabs; // List of stage prefabs in order
    public Transform spawnPoint;          // Location to spawn stages

    [Header("Trigger Settings")]
    public List<GameObject> stageTriggers; // List of trigger objects for each stage

    private int currentStageIndex = 0;    // Index of the current stage
    private GameObject currentStage;      // The currently active stage
    private GameObject previousStage;     // The last stage (for cleanup)

    private void Start()
    {
        // Spawn the initial stage and set up the first trigger
        if (stagePrefabs.Count > 0)
        {
            currentStage = Instantiate(stagePrefabs[currentStageIndex], spawnPoint.position, spawnPoint.rotation);
            ActivateTrigger(currentStageIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a valid trigger and the player
        if (other.gameObject == player && stageTriggers.Contains(other.gameObject))
        {
            LoadNextStage();
        }
    }

    private void LoadNextStage()
    {
        // Increment the stage index
        currentStageIndex++;

        // Destroy the previous stage
        if (previousStage != null)
        {
            Destroy(previousStage);
        }

        // Move the current stage to previous stage
        previousStage = currentStage;

        // Load the next stage if available
        if (currentStageIndex < stagePrefabs.Count)
        {
            currentStage = Instantiate(stagePrefabs[currentStageIndex], spawnPoint.position, spawnPoint.rotation);

            // Activate the corresponding trigger
            ActivateTrigger(currentStageIndex);
        }
        else
        {
            Debug.Log("No more stages to load.");
        }
    }

    private void ActivateTrigger(int index)
    {
        // Deactivate all triggers first
        foreach (GameObject trigger in stageTriggers)
        {
            trigger.SetActive(false);
        }

        // Activate the trigger for the current stage, if it exists
        if (index < stageTriggers.Count)
        {
            stageTriggers[index].SetActive(true);
        }
    }
}
