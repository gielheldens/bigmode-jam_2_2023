//using System.Numerics;
using System;
using Unity.VisualScripting;
using UnityEditor;
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
    [SerializeField] private float _slopeCheckDist;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Feet _feet;
    [SerializeField] private PhysicsMaterial2D _frictionless;
    [SerializeField] private PhysicsMaterial2D _friction;

    [Header ("Attributes")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header ("Tags")]
    [SerializeField] string drawManagerTag;

    // movement variables
    private float _horizontalInput;

    // private references
    private DrawManager _drawManager;
    private CapsuleCollider2D _collider;

    // collider attributes
    private Vector2 _colliderSize;

    // slope variables
    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _prevSlopeDownAngle;
    private Vector2 _slopeNormalPerp;
    private bool _onSlope;
    private bool _canSlope;
    

    void Start()
    {
        _drawManager = GameObject.FindWithTag(drawManagerTag).GetComponent<DrawManager>();
        _collider = GetComponent<CapsuleCollider2D>();
        _colliderSize = _collider.size;
    }


    private void Update()
    {
        Debug.Log(_drawManager.drawing);
        Inputs();
        AnimationState();
    }

    private void FixedUpdate()
    {
        Movement();
        SlopeCheck();
    }

    private void Inputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        Vector2 velocity = _rb2d.velocity;
        if (_drawManager.drawing)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            velocity = Vector2.zero;
        }
        else
        {
            _rb2d.bodyType = RigidbodyType2D.Dynamic;

            if (_feet.grounded && !_onSlope)
            {
                velocity = new Vector2(_horizontalInput * _moveSpeed, 0f);
            } 
            else if (_feet.grounded && _onSlope)
            {
                velocity = new Vector2(_moveSpeed * _slopeNormalPerp.x * -_horizontalInput, _moveSpeed * _slopeNormalPerp.y * -_horizontalInput);
            }
            else if (!_feet.grounded)
            {
                velocity.x = _horizontalInput * _moveSpeed;
            }
        }
        _rb2d.velocity = velocity;
    }

    private void SlopeCheck()
    {
        Vector2 _checkPos = transform.position - new Vector3(0f, _colliderSize.y / 2, 0f);
        SlopeCheckHor(_checkPos);
        SlopeCheckVert(_checkPos);
    }

    private void SlopeCheckHor(Vector2 _pos)
    {
        RaycastHit2D _hitFront = Physics2D.Raycast(_pos, transform.right * Facing(), _slopeCheckDist, _groundMask);
        RaycastHit2D _hitBack = Physics2D.Raycast(_pos, -transform.right * Facing(), _slopeCheckDist, _groundMask);

        if (_hitFront)
        {
            _onSlope = true;
            _slopeSideAngle = Vector2.Angle(_hitFront.normal, Vector2.up);
        }
        else if (_hitBack)
        {
            _onSlope = true;
            _slopeSideAngle = Vector2.Angle(_hitBack.normal, Vector2.up);
        }
        else
        {
            _onSlope = false;
            _slopeSideAngle = 0f;
        }

    }

    private void SlopeCheckVert(Vector2 _pos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_pos, Vector2.down, _slopeCheckDist, _groundMask);
        if(_hit) 
        {
            _slopeNormalPerp = Vector2.Perpendicular(_hit.normal).normalized;
            _slopeDownAngle = Vector2.Angle(_hit.normal, Vector2.up);
            
            if(_slopeDownAngle != _prevSlopeDownAngle) _onSlope = true;
            _prevSlopeDownAngle = _slopeDownAngle;

            Debug.DrawRay(_hit.point, _slopeNormalPerp, Color.red);
            Debug.DrawRay(_hit.point, _hit.normal, Color.green);
        }

        if(_slopeDownAngle > _maxSlopeAngle || _slopeSideAngle > _maxSlopeAngle) _canSlope = false;
        else _canSlope = true;

        if(_onSlope && _horizontalInput == 0f && _canSlope)
        {
            _rb2d.sharedMaterial = _friction;
        }
        else
        {
            _rb2d.sharedMaterial = _frictionless;
        }
    }

    private int Facing()
    {
        if(_sprite.flipX) return -1;
        else return 1;
    }

    private void AnimationState()
    {
        _animator.SetBool("drawing", false);
        _animator.SetBool("walking", false);
        if(_drawManager.drawing) _animator.SetBool("drawing", true);
        else if (Mathf.Abs(_horizontalInput) > 0f && _rb2d.velocity.y > -1f) 
        {
            _sprite.flipX = false;
            if(_horizontalInput < 0f) _sprite.flipX = true;
            _animator.SetBool("walking", true);
        }
    }
}