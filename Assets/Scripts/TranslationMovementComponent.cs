using UnityEngine;

// Moves an object in a given direction at a specified speed. This can be used for projectiles, enemies, or any object that needs to move.
public class TranslationMovementComponent : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Direction of movement
    public float speed = 5f; // Speed of movement
    public bool move = true; // If true, the object will start moving immediately
    void Start()
    {
        // Normalize the direction to ensure consistent movement speed
        direction = direction.normalized;
    }
    void FixedUpdate()
    {
        // Move the object in the specified direction at the specified speed
        if (move) Move();
        else return;
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
