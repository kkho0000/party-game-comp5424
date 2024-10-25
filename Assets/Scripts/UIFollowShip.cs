using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class UIFollowShip : MonoBehaviour
{
    public Transform spaceshipTransform; // Assign in the Inspector
    private Vector3 uiOffset;
    private Quaternion uiRoOffset;

    void Start()
    {
        // Initialize the offset between the UI and spaceship
        uiOffset = transform.position - spaceshipTransform.position;
        uiRoOffset = Quaternion.Euler(transform.rotation.eulerAngles - spaceshipTransform.rotation.eulerAngles);
    }

    void Update()
    {
        // Update the UI position
        transform.position = spaceshipTransform.position + uiOffset;

        // Update the UI rotation
        transform.rotation = spaceshipTransform.rotation * uiRoOffset;
    }
}