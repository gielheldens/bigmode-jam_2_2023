using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    // the _linePrefab has a Transform, Line Renderer, RigidBody2D, and Line (script) component
    [SerializeField] private Line _linePrefab;
    [SerializeField] private LayerMask _drawBoxLayer;
    [SerializeField] private string _lineName;

    [Header ("Attributes")]
    public Color[] colors;

    [Header ("Tags")]
    [SerializeField] private string _cameraTag;

    [Header ("Booleans")]
    public bool drawing;

    [Header ("Sounds")]
    [SerializeField] private AudioSource _drawingSound;

    // private references
    private Camera _cam;

    // draw variables
    private Line _currentLine;
    private Transform _hoverDrawParent = null;
    private Transform _drawParent = null;
    private bool _draw;
    private bool _playingDrawSound;

    // constant
     public const float RESOLUTION = 0.1f;


    void Start()
    {
        // finds and assigns camera by tag
        _cam = GameObject.FindWithTag(_cameraTag).GetComponent<Camera>();
    }

    void Update()
    {
        // convert mouse position on screen to world position
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        MouseInput();
        HoverDrawParent(mousePos, _drawBoxLayer);
        SetDrawParent();

        // if we want to draw (_draw is true) and we have a draw parent selected,
        // or if we were already drawing, call Draw method
        if((_draw && _drawParent!= null) || drawing) 
        {
            Draw(mousePos, _drawParent, DrawBox(_drawParent), DrawMode(_drawParent));
        }

        // if we were already drawing but not playing the draw sound, play draw sound.
        // else if not drawing, stop draw sound
        if (drawing && !_playingDrawSound) 
        {
            _playingDrawSound = true;
            _drawingSound.Play();
        }
        else if (!drawing) 
        {
            _playingDrawSound = false;
            _drawingSound.Stop();
        }
    }

    // check whether we want to draw, based on left mouse button presses
    private void MouseInput()
    {
        if(Input.GetMouseButtonDown(0)) _draw = true;

        if(Input.GetMouseButtonUp(0)) _draw = false;
    }

    // check whether an input position is overlapping a transform on the layer mask
    // used to determine if the mouse position overlaps with a draw parent
    private void HoverDrawParent(Vector2 _pos, LayerMask _layers)
    {
        if (Physics2D.OverlapPoint(_pos, _layers)) _hoverDrawParent = Physics2D.OverlapPoint(_pos, _layers).gameObject.transform.parent;
        else _hoverDrawParent = null;
    }

    // if we are not drawing and dont want to draw yet, the current 
    // draw parent is set to the hovered draw parent, even if it is null
    private void SetDrawParent()
    {
        if (!drawing && !_draw) _drawParent = _hoverDrawParent;
    }

    // takes the mousepos, the transform parent in the scene hierarchy, the 2D collider
    // of the draw box, and the mode of drawing to determine the drawing behaviour
    private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    {
        // if we want to draw and are not currently drawing
        if(_draw && !drawing)
        {   
            // instantiate a line prefab at the mouse position
            _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);
            // setting the color of the line prefab
            if (drawMode == RigidbodyType2D.Static) _currentLine.SetLineColor(colors, 0);
            else _currentLine.SetLineColor(colors, 1);
            if (parent.CompareTag("Final")) _currentLine.SetLineColor(colors, 2);
            // set drawing boolean to true
            drawing = true;
        }
        // if we want to draw and were already drawing
        else if (_draw && drawing)
        {
            // we call the SetPosition method from the line prefab,
            // using the mousePos and drawBox
            _currentLine.SetPosition(mousePos, drawBox);
        }
        else
        {
            // call destroy old line method, with the draw parent transform and line name
            DestroyOldLine(_drawParent, _lineName);
            // name the current line
            _currentLine.name = _lineName;
            // set drawing to falls
            drawing = false;
            // call line prefab methods to determine the physics of the drawn line
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.SetMass();
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
            _currentLine.SetBodyType(drawMode);
            // set draw parent to null
            _drawParent = null;
        }
    }

    private Collider2D DrawBox(Transform parent)
    {
        return parent.Find("Box").GetComponent<Collider2D>();
    }

    private RigidbodyType2D DrawMode(Transform parent)
    {
        if (parent.CompareTag("Static")) return RigidbodyType2D.Static;
        else if (parent.CompareTag("Final")) return RigidbodyType2D.Static;
        return RigidbodyType2D.Dynamic;
    }

    private void DestroyOldLine (Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            // Check if the child's name matches the target name
            if (child.name == name)
            {
                // Destroy the child object
                Destroy(child.gameObject);
            }
        }
    }
}
