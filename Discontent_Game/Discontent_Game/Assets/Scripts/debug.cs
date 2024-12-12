using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class debug : MonoBehaviour
{
    public Button myButton; // Reference to your button

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button reference is missing! Please assign the button in the Inspector.");
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }
}