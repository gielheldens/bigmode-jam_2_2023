using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    //[SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;

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
    private Transform _drawParent;


    void Start()
    {
        _cam = GameObject.FindWithTag(cameraTag).GetComponent<Camera>();
        _drawBoxes = GameObject.FindGameObjectsWithTag(drawBoxTag);
    }

    void Update()
    {
        if(_drawParent != null) Debug.Log("what is the drawparent? " + _drawParent.name);
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (!drawing) _drawParent = SetDrawParent(mousePos);

        if(_drawParent!= null) 
        {
            Draw(mousePos, _drawParent, DrawBox(_drawParent), DrawMode(_drawParent));
        }
    }

    private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    {
  
        if(Input.GetMouseButtonDown(0)) 
        {
            _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);
            DestroyOldLine(_drawParent, lineName);
            _currentLine.name = lineName;
            if (drawMode == RigidbodyType2D.Static) _currentLine.SetLineColor(colors, 0);
            else _currentLine.SetLineColor(colors, 1);
            drawing = true;
        }

        if(Input.GetMouseButton(0)) 
        {
            _currentLine.SetPosition(mousePos, drawBox);
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawing = false;
            _currentLine.GenerateColliders(_currentLine.points);
            
            _currentLine.SetMass();
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
            _currentLine.SetBodyType(drawMode);
            // DestroyOldLine(_drawParent, lineName);
            // _currentLine.name = lineName;
            _drawParent = null;
        }
    }

    private Collider2D DrawBox(Transform parent)
    {
        return parent.Find("Box").GetComponent<Collider2D>();
    }

    private Transform SetDrawParent(Vector2 pos)
    {
        foreach (GameObject obj in _drawBoxes)
        {
            Collider2D drawBox = obj.GetComponent<BoxCollider2D>();
            if (drawBox.enabled == true)
            {
                if (drawBox.bounds.Intersects(new Bounds(pos, Vector3.one))) return drawBox.transform.parent;
            }
        }
        return null;
    }

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
