using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    private float backgroundWidth; // Width of the background sprite

    void Start()
    {
        // Get the width of the background sprite by using its Renderer bounds
        backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        print(backgroundWidth);
    }

    void Update()
    {
        // Check if the background is off-screen and reposition it
        if (transform.localPosition.x < -backgroundWidth /4)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        // Move the background forward by the width to create the looping effect
        Vector3 offset = new Vector3(backgroundWidth / 2f, 0, 0);
        transform.position = (Vector3)transform.position + offset;
    }
}
