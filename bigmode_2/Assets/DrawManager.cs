using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Line _linePrefab;
    [SerializeField] private PlayerController playerController;

    [Header ("Attributes")]
    [SerializeField] private string lineName;

    [Header ("Tags")]
    [SerializeField] private string drawBoxTag;

    public const float RESOLUTION = 0.1f;

    // draw variables
    private GameObject[] _drawBoxes;
    public bool drawing;
    private Line _currentLine;
    private Transform _drawParent;


    void Start()
    {
        _drawBoxes = GameObject.FindGameObjectsWithTag(drawBoxTag);
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (!drawing) _drawParent = SetDrawParent(mousePos);
        

        if(SetDrawParent(mousePos) != null) Debug.Log(SetDrawParent(mousePos).name);

        if(_drawParent!= null) 
        {
            Draw(mousePos, _drawParent, DrawBox(_drawParent), DrawMode(_drawParent));
        }
    }

    private void Draw(Vector2 mousePos, Transform parent, Collider2D drawBox, RigidbodyType2D drawMode)
    {
  
        if(Input.GetMouseButtonDown(0)) _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity, parent);

        if(Input.GetMouseButton(0)) 
        {
            _currentLine.SetPosition(mousePos, drawBox);
            drawing = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawing = false;
            _currentLine.GenerateColliders(_currentLine.points);
            _currentLine.SetMass();
            _currentLine.GetComponent<Rigidbody2D>().gravityScale = 1;
            _currentLine.SetBodyType(drawMode);
            DestroyOldLine(_drawParent, lineName);
            _currentLine.name = lineName;
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
