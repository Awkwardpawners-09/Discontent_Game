using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop7Jumpscare : MonoBehaviour
{
    [Header("Objects to Assign")]
    public GameObject object1; // The object that will trigger the collision (Object #1)
    public GameObject object2; // The target object (Object #2)
    public GameObject object3; // The object to enable upon collision (Object #3)

    private void Start()
    {
        // Ensure that the necessary objects are assigned
        if (object1 == null || object2 == null || object3 == null)
        {
            Debug.LogWarning("Please make sure all objects are assigned in the Inspector!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if object1 collides with object2
        if (collision.gameObject == object2)
        {
            // Enable object3 when the collision happens
            if (object3 != null)
            {
                object3.SetActive(true);
                Debug.Log("Object #3 has been enabled.");
            }
        }
    }

    // If you're using triggers instead of collisions, use OnTriggerEnter instead
    private void OnTriggerEnter(Collider other)
    {
        // Check if object1 collides with object2
        if (other.gameObject == object2)
        {
            // Enable object3 when the collision happens
            if (object3 != null)
            {
                object3.SetActive(true);
                Debug.Log("Object #3 has been enabled.");
            }
        }
    }
}
