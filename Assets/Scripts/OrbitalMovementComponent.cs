using System;
using UnityEngine;

// Moves an object in an orbit around a specified center point at a given radius and speed. This can be used for planets, satellites, or any object that needs to orbit around another object.


[Serializable] public class OrbitalMovementComponent : MonoBehaviour
{
    [SerializeField, Tooltip("The point around which the object will orbit.")]
    Transform centerPoint;

    [SerializeField, Tooltip("The radius of the orbit.")]
    float radius = 5f;

    [SerializeField, Tooltip("The speed of the orbit. Positive values will orbit clockwise, negative values will orbit counterclockwise.")]
    float speed = 1f;

    [SerializeField, Tooltip("The initial angle of the orbit in degrees.")]
    float initialAngle = 0f;

    [SerializeField, Tooltip("If true, the object will always face the center point.")]
    bool tidalLock = true;

    [SerializeField, Tooltip("If true, the orbit will start automatically.")]
    bool autoStart = true;

    [SerializeField, Tooltip("Reference to the Animator component for playing animations.")]
    private Animator animator;

    #region Animation

    [SerializeField, Tooltip("The name of the animation bool to set when orbiting counterclockwise.")]
    string animationTriggerCounterclockwise = "OrbitingCounterclockwise";

    [SerializeField, Tooltip("The name of the animation bool to set when orbiting clockwise.")]
    string animationTriggerClockwise = "OrbitingClockwise";

    #endregion

    void Start()
    {
   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (autoStart)
        {
            Orbit();
        }

    }

    void Orbit() {

        if (animator != null)
        {
            if (animationTriggerClockwise == null || animationTriggerCounterclockwise == null) return; // Ensure there are animation triggers specified before trying to set them
            Debug.LogWarning("Animation triggers are not specified for the OrbitalMovementComponent. Please set the animationTriggerRight and animationTriggerLeft fields to use animations.");

            if (speed > 0) animator.SetTrigger(animationTriggerCounterclockwise); // Start the orbiting animation if an animation bool is specified
            else if (speed < 0) animator.SetTrigger(animationTriggerClockwise); // Start the orbiting animation if an animation bool is specified
        }
        else Debug.LogWarning("No Animator component found on the object with the OrbitalMovementComponent. Please add an Animator component and set the animator field to use animations.");


        if (centerPoint == null) return; // Ensure there is a center point to orbit around
        
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
