using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float horizontalInput;

    // draw variables
    public Transform drawParent;
    public int drawMode;     // 0 is none, 1 is static, 2 is dynamic

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

    void OnTriggerStay2D(Collider2D other)
    {
        drawParent = other.transform.parent;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        drawParent = null;
    }
}