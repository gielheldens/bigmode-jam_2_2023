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
    //public RigidbodyType2D drawMode;

    // line references
    private Line _currentLine;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Transform drawParent = playerController.drawParent;

        if(drawParent!= null) 
        {
            //drawMode = DrawMode(drawParent);
            Draw(mousePos, drawParent, DrawBox(drawParent), DrawMode(drawParent));
        }
    }

    private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    {
  
        if(Input.GetMouseButtonDown(0)) _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);

        if(Input.GetMouseButton(0)) 
        {
            _currentLine.SetPosition(mousePos, drawBox);
        }

        if(Input.GetMouseButtonUp(0))
        {
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
            _currentLine.SetBodyType(drawMode);
        }
    }

    private Collider2D DrawBox(Transform parent)
    {
        return parent.Find("Box").GetComponent<Collider2D>();
    }


    private RigidbodyType2D DrawMode(Transform parent)
    {
        if (parent.CompareTag("Static")) return RigidbodyType2D.Static;
        return RigidbodyType2D.Dynamic;
    }
}
