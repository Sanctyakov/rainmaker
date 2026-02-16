using UnityEngine;

// Moves an object in an orbit around a specified center point at a given radius and speed. This can be used for planets, satellites, or any object that needs to orbit around another object.

public class OrbitalMovementComponent : MonoBehaviour
{
    public Transform centerPoint; // The point around which the object will orbit
    public float radius = 5f; // The radius of the orbit
    public float speed = 1f; // The speed of the orbit
    public float initialAngle = 0f; // The initial angle of the orbit in degrees
    public bool tidalLock = false; // If true, the object will always face the center point

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
