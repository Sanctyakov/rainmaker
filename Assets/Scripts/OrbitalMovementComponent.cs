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

    [SerializeField, Tooltip("The speed at which the object will orbit automatically in degrees per second when auto is enabled.")]
    float autoSpeed = 50f;

    [SerializeField, Tooltip("Maximum orbit speed in degrees per second. Actual speed is taken from input * maxOrbitSpeed.")]
    float maxOrbitSpeed = 180f;

    [SerializeField, Tooltip("The initial angle of the orbit in degrees.")]
    float initialAngle = 0f;

    [SerializeField, Tooltip("If true, the object will always face the center point.")]
    bool tidalLock = true;

    [SerializeField, Tooltip("If true, the orbital movement will play automatically.")]
    bool auto = true;

    [SerializeField, Tooltip("Reference to the Animator component for playing animations.")]
    private Animator animator;

    [SerializeField, Tooltip("Input axis used to control orbit (left = clockwise, right = counterclockwise).")]
    string inputAxis = "Horizontal";

    [SerializeField, Tooltip("Input deadzone to prevent jitter when joystick is near center.")]
    float inputDeadzone = 0.05f;

    #region Animation

    [SerializeField, Tooltip("The name of the animation trigger to set when orbiting counterclockwise.")]
    string animationTriggerCounterclockwise = "Strafe left";

    [SerializeField, Tooltip("The name of the animation trigger to set when orbiting clockwise.")]
    string animationTriggerClockwise = "Strafe right";

    #endregion

    #region Rigidbody Settings

    Rigidbody rb;
    Vector3 lastCenterPosition;
    float currentAngle;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (orbitCenter == null)
        {
            Debug.LogError("No center point specified for the OrbitalMovementComponent. Please set the orbitCenter field to a valid Transform.");
            enabled = false;
            return;
        }

        currentAngle = initialAngle;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * radius;

        // Initialize position using Rigidbody when available, otherwise transform
        if (rb != null)
        {
            rb.position = orbitCenter.position + offset;
        }
        else
        {
            transform.position = orbitCenter.position + offset;
        }

        lastCenterPosition = orbitCenter.position;
    }

    void FixedUpdate()
    {
        if (auto) Orbit(autoSpeed);
        else
        {
            // Read input and convert to an applied speed in degrees/sec.
            float rawInput = 0f;
            try
            {
                rawInput = Input.GetAxis(inputAxis);
            }
            catch (Exception)
            {
                rawInput = 0f;
            }

            if (Mathf.Abs(rawInput) < inputDeadzone) rawInput = 0f;

            // Mapping: left -> clockwise, right -> counterclockwise.
            // Unity Horizontal: left = -1, right = +1.
            // We want clockwise => positive speed, so invert input.
            float appliedSpeed = -rawInput * maxOrbitSpeed;

            Orbit(appliedSpeed);
        }   
    }

    void Orbit(float appliedSpeed)
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
                // appliedSpeed > 0 => clockwise (left); appliedSpeed < 0 => counterclockwise (right)
                if (appliedSpeed > 0f) animator.SetTrigger(animationTriggerClockwise);
                else if (appliedSpeed < 0f) animator.SetTrigger(animationTriggerCounterclockwise);
            }
        }

        // Update the orbital angle (appliedSpeed is degrees per second)
        currentAngle += appliedSpeed * Time.fixedDeltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;

        // Desired position on orbit
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * radius;
        Vector3 desiredPosition = orbitCenter.position + offset;

        // Compute center velocity (handles moving orbit center)
        Vector3 centerVelocity = (orbitCenter.position - lastCenterPosition) / Time.fixedDeltaTime;
        lastCenterPosition = orbitCenter.position;

        if (rb != null)
        {
            // Tangential (orbital) velocity: v = omega x r, where omega = appliedSpeed (deg/s) -> convert to rad/s
            float omega = appliedSpeed * Mathf.Deg2Rad;
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