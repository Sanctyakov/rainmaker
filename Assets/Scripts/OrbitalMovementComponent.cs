using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class OrbitalMovementComponent : MonoBehaviour
{
    [SerializeField, Tooltip("The point around which the object will orbit.")]
    Transform orbitCenter;

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

    Rigidbody rb;
    Vector3 lastCenterPosition;
    float currentAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("OrbitalMovementComponent requires a Rigidbody on the same GameObject.");
            enabled = false;
            return;
        }

        if (orbitCenter == null)
        {
            Debug.LogError("No center point specified for the OrbitalMovementComponent. Please set the centerPoint field to a valid Transform.");
            enabled = false;
            return;
        }

        currentAngle = initialAngle;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * radius;
        rb.position = orbitCenter.position + offset;
        lastCenterPosition = orbitCenter.position;
    }

    void FixedUpdate()
    {
        if (autoStart)
        {
            Orbit();
        }
    }

    void Orbit()
    {
        if (orbitCenter == null) return;

        // Animation triggers (only when animator is present)
        if (animator != null)
        {
            if (string.IsNullOrWhiteSpace(animationTriggerClockwise) || string.IsNullOrWhiteSpace(animationTriggerCounterclockwise))
            {
                Debug.LogWarning("Animation triggers are not specified for the OrbitalMovementComponent. Please set the animationTriggerClockwise and animationTriggerCounterclockwise fields to use animations.");
            }
            else
            {
                if (speed > 0f) animator.SetTrigger(animationTriggerCounterclockwise);
                else if (speed < 0f) animator.SetTrigger(animationTriggerClockwise);
            }
        }

        // Update the orbital angle (speed is degrees per second)
        currentAngle += speed * Time.fixedDeltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;

        // Desired position on orbit
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * radius;
        Vector3 desiredPosition = orbitCenter.position + offset;

        // Compute center velocity (handles moving orbit center)
        Vector3 centerVelocity = (orbitCenter.position - lastCenterPosition) / Time.fixedDeltaTime;
        lastCenterPosition = orbitCenter.position;

        if (rb != null)
        {
            // Tangential (orbital) velocity: v = omega x r, where omega = speed (deg/s) -> convert to rad/s
            float omega = speed * Mathf.Deg2Rad;
            Vector3 tangentialDirection = new Vector3(-Mathf.Sin(radians), 0f, Mathf.Cos(radians)); // perpendicular to radius
            Vector3 orbitalVelocity = tangentialDirection * (radius * omega);

            // Final world velocity
            Vector3 finalVelocity = orbitalVelocity + centerVelocity;

            // Apply velocity to Rigidbody (physics-driven movement)
            rb.linearVelocity = finalVelocity;

            // Tidal lock: rotate to face center using physics-friendly rotation
            if (tidalLock)
            {
                Vector3 toCenter = orbitCenter.position - rb.position;
                if (toCenter.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(toCenter);
                    rb.MoveRotation(targetRotation);
                }
            }
        }
        else
        {
            // Fallback: direct transform updates when there's no Rigidbody
            transform.position = desiredPosition;

            if (tidalLock)
            {
                transform.LookAt(orbitCenter);
            }
        }
    }
}