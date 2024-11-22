using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCamera : MonoBehaviour
{
    public Transform cameraTransform; // Drag and drop your camera object here
    public Vector3 positionOffset; // Set this in the Inspector to adjust the position
    public Vector3 rotationOffset; // Set this in the Inspector to adjust the rotation

    void LateUpdate()
    {
        // Follow the camera's position with an offset
        transform.position = cameraTransform.position + positionOffset;

        // Align the hand's rotation with the camera's rotation plus an offset
        transform.rotation = cameraTransform.rotation * Quaternion.Euler(rotationOffset);
    }
}
