using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowShip : MonoBehaviour
{
    public Transform spaceshipTransform; // Assign in the Inspector
    private Vector3 uiOffset;

    void Start()
    {
        // Initialize the offset between the UI and spaceship
        uiOffset = transform.position - spaceshipTransform.position;
    }

    void Update()
    {
        // Update the UI position
        transform.position = spaceshipTransform.position + uiOffset;

        // Update the UI rotation
        transform.rotation = spaceshipTransform.rotation;
    }
}
