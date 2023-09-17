using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Transform rotationPoint; // The point you want to rotate around
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    void Update()
    {
        // Ensure that rotationPoint is not null
        if (rotationPoint == null)
        {
            Debug.LogError("Rotation point is not assigned!");
            return;
        }

        // Calculate the rotation axis (usually up in world space)
        Vector3 axis = Vector3.up;

        // Calculate the rotation angle based on the speed and time
        float angle = rotationSpeed * Time.deltaTime;

        // Use RotateAround to rotate the object around the rotation point
        transform.RotateAround(rotationPoint.position, axis, angle);
    }
}
