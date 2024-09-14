using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform[] backgroundLayers;  // Array of background layers
    public float[] parallaxScales;        // Proportion of camera movement to move the backgrounds
    public float smoothing = 1f;          // Smoothing factor for parallax effect

    private Transform cameraTransform;    // Reference to the camera transform
    private Vector3 previousCameraPosition; // Store the previous frame camera position

    void Start()
    {
        // Get the camera transform and initialize the previousCameraPosition
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        // If the parallaxScales array is not set, initialize it to match the layers
        if (parallaxScales.Length != backgroundLayers.Length)
        {
            parallaxScales = new float[backgroundLayers.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = backgroundLayers[i].position.z * -1;
            }
        }
    }

    void FixedUpdate()
    {
        // Loop through each background layer and apply parallax effect
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            // Calculate the parallax based on the camera movement
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // Determine the target position of the background layer
            float backgroundTargetPosX = backgroundLayers[i].position.x + parallax;

            // Apply a smooth transition to the target position
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPosX, backgroundLayers[i].position.y, backgroundLayers[i].position.z);

            // Move the background layer to its new position
            backgroundLayers[i].position = Vector3.Lerp(backgroundLayers[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);
        }

        // Update the previous camera position to the current position for the next frame
        previousCameraPosition = cameraTransform.position;
    }
}