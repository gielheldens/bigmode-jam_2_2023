using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    //[SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;
    [SerializeField] private LayerMask _drawBoxLayer;

    [Header ("Attributes")]
    [SerializeField] private string lineName;
    public Color[] colors;

    [Header ("Tags")]
    [SerializeField] private string drawBoxTag;
    [SerializeField] private string cameraTag;

    [Header ("Booleans")]
    public bool drawing;

    // private references
    private Camera _cam;

    public const float RESOLUTION = 0.1f;

    // draw variables
    private GameObject[] _drawBoxes;
    private Line _currentLine;
    private Transform _hoverDrawParent = null;
    private Transform _drawParent = null;
    //private Transform _activeDrawParent;
    private bool _draw;


    void Start()
    {
        _cam = GameObject.FindWithTag(cameraTag).GetComponent<Camera>();
        _drawBoxes = GameObject.FindGameObjectsWithTag(drawBoxTag);
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        MouseInput();
        HoverDrawParent(mousePos, _drawBoxLayer); // _drawBoxLayer);
        SetDrawParent();

        if(_drawParent != null) Debug.Log("we have a parent " + _drawParent.name);
        if(_hoverDrawParent != null) Debug.Log("we hover a parent " + _hoverDrawParent.name);

        if((_draw && _drawParent!= null) || drawing) 
        {
            Draw(mousePos, _drawParent, DrawBox(_drawParent), DrawMode(_drawParent));
        }
    }

    private void MouseInput()
    {
        if(Input.GetMouseButtonDown(0)) _draw = true;

        if(Input.GetMouseButtonUp(0)) _draw = false;
    }

    private void SetDrawParent()
    {
        if (!drawing && !_draw) _drawParent = _hoverDrawParent;
    }

    private void HoverDrawParent(Vector2 _pos, LayerMask _layers)
    {
        if (Physics2D.OverlapPoint(_pos, _layers)) _hoverDrawParent = Physics2D.OverlapPoint(_pos, _layers).gameObject.transform.parent;
        else _hoverDrawParent = null;
    }

    private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    {
        if(_draw && !drawing)
        {
            DestroyOldLine(_drawParent, lineName);
            _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);
            _currentLine.name = lineName;
            if (drawMode == RigidbodyType2D.Static) _currentLine.SetLineColor(colors, 0);
            else _currentLine.SetLineColor(colors, 1);
            drawing = true;
        }
        else if (_draw && drawing)
        {
            _currentLine.SetPosition(mousePos, drawBox);
        }
        else
        {
            drawing = false;
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.SetMass();
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
            _currentLine.SetBodyType(drawMode);
            _drawParent = null;
        }
    }

    // private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    // {
  
    //     if(Input.GetMouseButtonDown(0)) 
    //     {
    //         DestroyOldLine(_drawParent, lineName);
    //         _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);
    //         _currentLine.name = lineName;
    //         if (drawMode == RigidbodyType2D.Static) _currentLine.SetLineColor(colors, 0);
    //         else _currentLine.SetLineColor(colors, 1);
    //         drawing = true;
    //     }

    //     if(Input.GetMouseButton(0)) 
    //     {
    //         _currentLine.SetPosition(mousePos, drawBox);
    //     }

    //     if(Input.GetMouseButtonUp(0))
    //     {
    //         drawing = false;
    //         _currentLine.GenerateColliders(_currentLine.points);
            
    //         _currentLine.SetMass();
    //         _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
    //         _currentLine.SetBodyType(drawMode);
    //         _activeDrawParent = _drawParent;
    //     }
    // }

    private Collider2D DrawBox(Transform parent)
    {
        return parent.Find("Box").GetComponent<Collider2D>();
    }

    // private Transform SetDrawParent(Vector2 pos)
    // {
    //     foreach (GameObject obj in _drawBoxes)
    //     {
    //         Collider2D drawBox = obj.GetComponent<BoxCollider2D>();
    //         if (drawBox.enabled == true)
    //         {
    //             if (drawBox.bounds.Intersects(new Bounds(pos, Vector3.one))) return drawBox.transform.parent;
    //         }
    //     }
    //     return null;
    // }

    private RigidbodyType2D DrawMode(Transform parent)
    {
        if (parent.CompareTag("Static")) return RigidbodyType2D.Static;
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
