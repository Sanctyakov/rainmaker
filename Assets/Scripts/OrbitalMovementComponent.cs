using UnityEngine;

// Moves an object in an orbit around a specified center point at a given radius and speed. This can be used for planets, satellites, or any object that needs to orbit around another object.

public class OrbitalMovementComponent : MonoBehaviour
{
    public Transform centerPoint; // The point around which the object will orbit
    public float radius = 5f; // The radius of the orbit
    public float speed = 1f; // The speed of the orbit
    public float initialAngle = 0f; // The initial angle of the orbit in degrees
    public bool tidalLock = false; // If true, the object will always face the center point
    public bool autoStart = true; // If true, the orbit will start automatically

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (autoStart)
        {
            AutoOrbit();
        }

    }

    void AutoOrbit() {         if (centerPoint == null) return; // Ensure there is a center point to orbit around
        // Calculate the current angle based on time and speed
        float angle = initialAngle + Time.time * speed;
        float radians = angle * Mathf.Deg2Rad; // Convert angle to radians
        // Calculate the new position in the orbit
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * radius;
        transform.position = centerPoint.position + offset;
        // If tidal lock is enabled, rotate the object to always face the center point
        if (tidalLock)
        {
            transform.LookAt(centerPoint);
        }
    }
}
