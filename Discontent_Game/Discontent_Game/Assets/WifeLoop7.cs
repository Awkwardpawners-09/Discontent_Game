using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeLoop7 : MonoBehaviour
{
    public GameObject movingObject; // Object #1, the one that will move (Ghost)
    public GameObject targetObject; // Object #2, the target to move towards (Player)
    public GameObject playerObject; // The Player object that will trigger the movement
    public float moveSpeed = 2f; // Speed of movement
    public GameObject deactivateOnCollision; // Object to deactivate upon collision with targetObject

    private bool isMoving = false; // Flag to track if movingObject is moving

    // Update is called once per frame
    void Update()
    {
        // If moving, move movingObject slowly towards targetObject
        if (isMoving && movingObject != null && targetObject != null)
        {
            movingObject.transform.position = Vector3.MoveTowards(
                movingObject.transform.position,
                targetObject.transform.position,
                moveSpeed * Time.deltaTime
            );

            // Check if movingObject has reached targetObject
            if (Vector3.Distance(movingObject.transform.position, targetObject.transform.position) < 0.1f)
            {
                OnCollisionWithTarget();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Player object collides with targetObject
        if (other.gameObject == playerObject)
        {
            // Start moving the ghost towards the target
            isMoving = true;
        }
    }

    private void OnCollisionWithTarget()
    {
        // Stop the movement when the ghost reaches the target
        isMoving = false;

        // Deactivate the specified object upon reaching the target
        if (deactivateOnCollision != null)
        {
            deactivateOnCollision.SetActive(false);
        }
    }
}
