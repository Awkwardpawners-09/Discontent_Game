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

// Start is called before the first frame update
void Start()
{
    
}

// Update is called once per frame
void Update()
{
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
}
}
