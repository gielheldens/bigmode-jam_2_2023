using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Track input in the Update function
        horizontalInput = Input.GetAxis("Horizontal");
        
        // You can perform non-physics related updates in Update
    }

    private void FixedUpdate()
    {
        // Physics-related updates should be done in FixedUpdate

        // Update velocity based on input
        Vector2 velocity = rb.velocity;
        velocity.x = horizontalInput * moveSpeed;
        rb.velocity = velocity;
    }
}