//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private DrawManager _drawManager;
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

    // draw variables
    private bool _wasDrawing;

    // movement variables
    private float horizontalInput;

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
        if(!_wasDrawing)
        {
            if(_drawManager.drawing)
            {
                _wasDrawing = true;
                _animator.Play(_enterDraw);
            }
            else if(MathF.Abs(horizontalInput) > 0f) 
            {
                _sprite.flipX = false;
                _animator.Play(_playerWalk);
                if(horizontalInput < 0f) _sprite.flipX = true;
            }
            else _animator.Play(_playerIdle);
        }
        else if (!_drawManager.drawing) 
        {
            Debug.Log("do we get here then?");
            _animator.Play(_exitDraw);
        }
        _wasDrawing = _drawManager.drawing;
    }

    private void EnterDrawState()
    {
        _animator.Play(_playerDraw);
    }

    private void EnterIdle()
    {
        Debug.Log("do we get here?");
        _animator.Play(_playerIdle);
    }

}