using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayonScreen : MonoBehaviour
{
    public Vector2 screenPosition = new Vector2(0.5f, 0.5f); // Normalized screen position (0,0 bottom-left to 1,1 top-right)
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Convert normalized screen position to canvas position
        Vector2 canvasPosition = new Vector2(
            screenPosition.x * Screen.width,
            screenPosition.y * Screen.height);

        rectTransform.position = canvasPosition;
    }
}
