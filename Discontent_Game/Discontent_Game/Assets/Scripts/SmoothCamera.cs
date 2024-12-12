using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;              // Target (player) to follow
    public float followSpeed = 5f;        // Speed for the camera to follow the player’s position
    public float rotationLagSpeed = 2f;   // Speed for the camera to catch up to the player’s rotation

    private Vector3 offset;               // Offset distance between camera and player

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
        else
        {
            Debug.LogWarning("No target assigned to the camera.");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Smoothly follow the player's position with offset
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Smoothly rotate to face the player's direction with a delay
            Quaternion targetRotation = Quaternion.LookRotation(target.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationLagSpeed * Time.deltaTime);
        }
    }
}
