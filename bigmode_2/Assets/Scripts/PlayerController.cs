//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

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
    [SerializeField] private Feet _feet;


    [Header ("Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _walkAnimCutoff;

    [Header ("Tags")]
    [SerializeField] string drawManagerTag;

    // movement variables
    private float _horizontalInput;
    private float _moveTimer;
    //private bool _canMove;

    // private references
    private DrawManager _drawManager;

    void Start()
    {
        _drawManager = GameObject.FindWithTag(drawManagerTag).GetComponent<DrawManager>();
    }


    private void Update()
    {
        Debug.Log(_rb2d.velocity.y);
        Movement();
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
            if (_feet.canMove)
            {
                velocity.x = _horizontalInput * moveSpeed;
                _rb2d.velocity = velocity;
            }
            
        }
    }

    private void Movement()
    {
        if (_feet.canMove) _horizontalInput = Input.GetAxisRaw("Horizontal");
        //else _horizontalInput = Mathf.Lerp(_horizontalInput, 0f, _lerpSpeed * Time.deltaTime);
        
    }

    private void AnimationState()
    {
        _animator.SetBool("drawing", false);
        _animator.SetBool("walking", false);
        if(_drawManager.drawing) _animator.SetBool("drawing", true);
        else if (Mathf.Abs(_horizontalInput) > 0f && _feet.canMove) 
        {
            _sprite.flipX = false;
            if(_horizontalInput < 0f) _sprite.flipX = true;
            _animator.SetBool("walking", true);
        }
    }


}