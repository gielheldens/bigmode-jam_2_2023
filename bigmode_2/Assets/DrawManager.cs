using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;

    public const float RESOLUTION = 0.1f;

    // line references
    private Line _currentLine;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mouseScreenPos = Input.mousePosition;
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        //MouseRaycast(mousePos);

        if(Input.GetMouseButtonDown(0)) _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity);

        if(Input.GetMouseButton(0)) 
        {
            _currentLine.SetPosition(mousePos);
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Debug.Log("do we keep getting here? and with how many points? " + _currentLine.points );
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
