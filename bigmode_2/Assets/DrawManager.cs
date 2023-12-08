using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;
    [SerializeField] private PlayerController playerController;

    public const float RESOLUTION = 0.1f;

    // draw variables
    public Collider2D currentDrawBox;
    public string drawMode;
    private Transform drawParent;

    // line references
    private Line _currentLine;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        drawParent = playerController.drawParent;

        DrawBox();
        if(drawParent!= null) 
        {
            drawMode = DrawMode(drawParent);
            Draw(mousePos);
        }
        
    }

    private void Draw(Vector2 mousePos)
    {
  
        if(Input.GetMouseButtonDown(0)) _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, drawParent);

        if(Input.GetMouseButton(0)) 
        {
            _currentLine.SetPosition(mousePos, currentDrawBox);
        }

        if(Input.GetMouseButtonUp(0))
        {
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

    private void DrawBox()
    {
        currentDrawBox = null;
        if (drawParent != null) currentDrawBox = drawParent.Find("Box").GetComponent<Collider2D>();
    }


    private string DrawMode(Transform parent)
    {
        if (parent.CompareTag("Static")) return "Static";
        return "Dynamic";
    }
}
