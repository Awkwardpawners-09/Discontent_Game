using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouselook : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor so it can move freely
        Cursor.visible = true; // Show the cursor
    }

    void Update()
    {
        // Do nothing to disable camera movement
    }
}
