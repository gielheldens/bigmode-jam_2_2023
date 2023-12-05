using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;
    [SerializeField] private Transform lineParent;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private EdgeCollider2D _collider;

    [Header ("Attributes")]
    [SerializeField] private float _lineWidth;

    public const float RESOLUTION = 0.05f;

    // line variables
    private Line _currentLine;
    private bool drawing;
    private bool once = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)) 
        {
            drawing = true;
            _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, lineParent);
        }


        if(Input.GetMouseButton(0)) _currentLine.SetPosition(mousePos);

        if(Input.GetMouseButtonUp(0)) drawing = false;

        if(_currentLine != null && !drawing && once) 
        {
            _currentLine.SetCollider(_currentLine._points);
            _currentLine.transform.SetParent(lineParent, false);
            once = false;
            rb2d.gravityScale = 1f;
        }

        // if(_currentLine != null && !drawing)
        // {
        //     for (int i = 0; i < _collider.points.Length; i++)
        //     {
        //         Debug.Log(" point " + i + " is " + _collider.points[i]);
        //         _currentLine.UpdatePosition(i, _collider.points[i]);
        //     }
        // }
        
    }
}
