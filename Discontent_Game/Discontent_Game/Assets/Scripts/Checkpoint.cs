using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.UpdateCheckpoint(transform.position);
            Debug.Log("Checkpoint reached!");
        }
    }
}
