using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float Drag = 0.98f;
    public float SteerAngle = 20;
    public float Traction = 1;

    public Transform[] wheels;

    private Vector3 MoveForce;

    private void Update()
    {   // Moving
        MoveForce += transform.forward * MoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        // Steering
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        // Drag and max speed limit
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Traction
        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;

        float speed = MoveForce.magnitude;
        foreach (Transform wheel in wheels)
        {
            if (Input.GetAxis("Vertical") < 0) // If driving in reverse
            {
                // Rotate back wheels in the opposite direction
                wheel.Rotate(Vector3.left, speed * 100f * Time.deltaTime);
            }
            else
            {
                wheel.Rotate(Vector3.right, speed * 100f * Time.deltaTime);
            }
        }
    }
}
