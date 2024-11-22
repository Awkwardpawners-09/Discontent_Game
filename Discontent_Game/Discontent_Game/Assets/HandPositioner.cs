using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositioner : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera; // Drag and drop your camera here in the Inspector
    public Transform handObject; // Drag and drop your hand object here in the Inspector

    [Header("Rotation Settings")]
    public Vector3 rotationOffset = new Vector3(0f, 180f, 0f); // Default rotation offset (Euler angles)

    void LateUpdate()
    {
        // Make the hand object face the same direction as the camera with the added rotation offset
        Quaternion targetRotation = playerCamera.transform.rotation * Quaternion.Euler(rotationOffset);
        handObject.rotation = targetRotation;

        // Optionally: If you want the hand to follow the camera position
        // handObject.position = playerCamera.transform.position; // Uncomment if you want to follow position
    }
}
