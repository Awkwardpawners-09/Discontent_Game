using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Assign the camera transform here
    public float delay = 0.3f; // Adjust this for more or less delay

    private Vector3 velocityX = Vector3.zero;
    private Vector3 velocityY = Vector3.zero;
    private Vector3 velocityZ = Vector3.zero;

    void Update()
    {
        // Interpolate each axis independently to achieve horizontal and vertical lagging
        Vector3 targetPosition = cameraTransform.position;

        float posX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocityX.x, delay);
        float posY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocityY.y, delay);
        float posZ = Mathf.SmoothDamp(transform.position.z, targetPosition.z, ref velocityZ.z, delay);

        transform.position = new Vector3(posX, posY, posZ);

        // Optionally match rotation with delay
        Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / delay);
    }
}
