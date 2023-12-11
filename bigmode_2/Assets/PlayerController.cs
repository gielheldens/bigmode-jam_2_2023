//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private DrawManager drawManager;
    [SerializeField] private Rigidbody2D rb2d;


    [Header ("Attributes")]
    [SerializeField]
    private float moveSpeed = 5f;

    // movement variables
    private float horizontalInput;

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb2d.velocity;
        if (drawManager.drawing)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            velocity.x = horizontalInput * moveSpeed;
            rb2d.velocity = velocity;
        }
    }
}