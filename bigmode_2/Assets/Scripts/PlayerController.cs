using System;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("References")]
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
    [SerializeField] private string _drawManagerTag;
    [SerializeField] private string _uiManagerTag;

    // movement variables
    private float _horizontalInput;

    // private references
    private DrawManager _drawManager;
    private CapsuleCollider2D _collider;
    private UIManager _uiManager;

    // collider attributes
    private Vector2 _colliderSize;

    // slope variables
    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _prevSlopeDownAngle;
    private Vector2 _slopeNormalPerp;
    private bool _onSlope;
    private bool _canClimb;
    

    // initialize references to other components and managers
    void Start()
    {
        _drawManager = GameObject.FindWithTag(_drawManagerTag).GetComponent<DrawManager>();
        _uiManager = GameObject.FindWithTag(_uiManagerTag).GetComponent<UIManager>();
        // this is the player collider and player collider size
        _collider = GetComponent<CapsuleCollider2D>();
        _colliderSize = _collider.size;
    }


    private void Update()
    {
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
        // process movement input unless the UI menu is active
        if(!_uiManager.inMenu) _horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        // adjust the player's velocity based on input and whether they're drawing
        Vector2 velocity = _rb2d.velocity;
        if (_drawManager.drawing)
        {
            // if drawing, stop player movement by making the player's Rigidbody kinematic
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            velocity = Vector2.zero;
        }
        else
        {
            // else, allow dynamic movement and apply horizontal input
            _rb2d.bodyType = RigidbodyType2D.Dynamic;

            // adjust movement based on whether the player is on a slope
            if (_feet.grounded && !_onSlope)    // on flat ground
            {
                velocity = new Vector2(_horizontalInput * _moveSpeed, 0f);
            } 
            else if (_feet.grounded && _onSlope)    // on a slope
            {
                velocity = new Vector2(_moveSpeed * _slopeNormalPerp.x * -_horizontalInput, _moveSpeed * _slopeNormalPerp.y * -_horizontalInput);
            }
            else if (!_feet.grounded)   // in the air
            {
                velocity = new Vector2(_horizontalInput * _moveSpeed, _rb2d.velocity.y);
            }
        }
        // apply calculated velocity
        _rb2d.velocity = velocity;
    }

    // perform checks to determine if the player is on a slope and adjust movement accordingly
    private void SlopeCheck()
    {
        Vector2 _checkPos = transform.position - new Vector3(0f, _colliderSize.y / 2, 0f);
        SlopeCheckHor(_checkPos);
        SlopeCheckVert(_checkPos);
    }

    // check for slopes horizontally to adjust for side slopes
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

    // check for slopes vertically to adjust movement and detect slope angle
    private void SlopeCheckVert(Vector2 _pos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_pos, Vector2.down, _slopeCheckDist, _groundMask);
        if(_hit) 
        {
            // calculate the perpendicular slope normal and angle
            _slopeNormalPerp = Vector2.Perpendicular(_hit.normal).normalized;
            _slopeDownAngle = Vector2.Angle(_hit.normal, Vector2.up);
            
            // update slope status based on angle change
            if(_slopeDownAngle != _prevSlopeDownAngle) _onSlope = true;
            _prevSlopeDownAngle = _slopeDownAngle;

            Debug.DrawRay(_hit.point, _slopeNormalPerp, Color.red);
            Debug.DrawRay(_hit.point, _hit.normal, Color.green);
        }

        // determining whether the player can climb the slope based on max slope angle
        if(_slopeDownAngle > _maxSlopeAngle || _slopeSideAngle > _maxSlopeAngle) _canClimb = false;
        else _canClimb = true;

        // set the player's physics material to either full friction or frictionless 
        // based on the following conditions
        if(_onSlope && _horizontalInput == 0 && _canClimb)
        {
            _rb2d.sharedMaterial = _friction;
        }
        else
        {
            _rb2d.sharedMaterial = _frictionless;
        }
    }

    // return the facing direction of the player baesd on the orientation
    // of the player sprite
    private int Facing()
    {
        if(_sprite.flipX) return -1;
        else return 1;
    }

    private void AnimationState()
    {
        // set animation states and player sprite orientation based on 
        // the player's current state and player inputs
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