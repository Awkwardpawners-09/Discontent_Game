using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("References")]
    public GameObject targetObject; // The object to follow (can be set in the inspector)
    public GameObject objectToFollow; // The object that will follow the target (can be set in the inspector)

    void Update()
    {
        if (targetObject != null && objectToFollow != null)
        {
            // Update position and rotation of the objectToFollow to match targetObject
            objectToFollow.transform.position = targetObject.transform.position;
            objectToFollow.transform.rotation = targetObject.transform.rotation;
        }
    }
}
