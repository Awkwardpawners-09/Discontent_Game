using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void QuitApplication()
    {
        Application.Quit();
        Debug.Log("Application has quit."); // This works only in the editor to confirm the function.
    }
}

