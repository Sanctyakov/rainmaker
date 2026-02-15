using UnityEngine;

// Moves an object in a given direction at a specified speed. This can be used for projectiles, enemies, or any object that needs to move automatically.
public class AutoMoveComponent : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Direction of movement
    public float speed = 5f; // Speed of movement
    void Start()
    {
        // Normalize the direction to ensure consistent movement speed
        direction = direction.normalized;
    }
    void Update()
    {
        // Move the object in the specified direction at the specified speed
        transform.position += direction * speed * Time.deltaTime;
    }
}
