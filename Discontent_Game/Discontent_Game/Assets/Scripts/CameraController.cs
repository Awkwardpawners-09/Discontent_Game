using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _camera;
    public float cameraSensitivity = 200.0f;
    public float cameraAccelaration = 5.0f;

    public Transform hand;

    private float rotation_x_axis;
    private float rotation_y_axis;

    // Zoom Variables
    public bool enableZoom = true;
    public float zoomedFOV = 30.0f; // Field of view when zoomed in
    public float defaultFOV = 60.0f; // Default field of view
    public float zoomSpeed = 5.0f; // Speed of zoom transition
    private Camera cameraComponent;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Camera component
        cameraComponent = _camera.GetComponent<Camera>();
        if (cameraComponent == null)
        {
            Debug.LogError("No Camera component found on the '_camera' Transform!");
            enableZoom = false;
        }
        else
        {
            cameraComponent.fieldOfView = defaultFOV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Camera rotation
        rotation_x_axis += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotation_y_axis += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;

        rotation_x_axis = Mathf.Clamp(rotation_x_axis, -70.0f, 90.0f);

        hand.localRotation = Quaternion.Euler(-rotation_x_axis, rotation_y_axis, 0);

        // Corrected the Quaternion.Euler usage for player rotation
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(0, rotation_y_axis, 0),
            cameraAccelaration * Time.deltaTime
        );

        // Corrected the Quaternion.Euler usage for camera rotation
        _camera.localRotation = Quaternion.Slerp(
            _camera.localRotation,
            Quaternion.Euler(-rotation_x_axis, 0, 0),
            cameraAccelaration * Time.deltaTime
        );

        // Handle zoom functionality
        if (enableZoom)
        {
            HandleZoom();
        }
    }

    private void HandleZoom()
    {
        if (Input.GetMouseButton(1)) // Right mouse button pressed
        {
            // Smoothly transition to zoomed FOV
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, zoomedFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            // Smoothly transition back to default FOV
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
        }
    }
}
