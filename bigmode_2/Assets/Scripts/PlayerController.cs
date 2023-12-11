//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header ("References")]
    //[SerializeField] private DrawManager _drawManager;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private string _playerWalk;
    [SerializeField] private string _playerIdle;
    [SerializeField] private string _playerDraw;
    [SerializeField] private string _enterDraw;
    [SerializeField] private string _exitDraw;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;


    [Header ("Attributes")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Header ("Tags")]
    [SerializeField] string drawManagerTag;

    // movement variables
    private float horizontalInput;

    // private references
    private DrawManager _drawManager;

    void Start()
    {
        _drawManager = GameObject.FindWithTag(drawManagerTag).GetComponent<DrawManager>();
    }


    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        AnimationState();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _rb2d.velocity;
        if (_drawManager.drawing)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            _rb2d.velocity = Vector2.zero;
        }
        else
        {
            _rb2d.bodyType = RigidbodyType2D.Dynamic;
            velocity.x = horizontalInput * moveSpeed;
            _rb2d.velocity = velocity;
        }
    }

    private void AnimationState()
    {
        _animator.SetBool("drawing", false);
        _animator.SetBool("walking", false);
        if(_drawManager.drawing) _animator.SetBool("drawing", true);
        else if (MathF.Abs(horizontalInput) > 0f) 
        {
            _sprite.flipX = false;
            if(horizontalInput < 0f) _sprite.flipX = true;
            _animator.SetBool("walking", true);
        }
    }
}