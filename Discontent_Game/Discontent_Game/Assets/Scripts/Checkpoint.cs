using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public JumpscareTrigger jumpscareScript; // Reference to JumpscareTrigger script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it only reacts to the player
        {
            jumpscareScript.UpdateCheckpoint(transform.position);
            Debug.Log("Checkpoint reached at position: " + transform.position);
        }
    }
}
