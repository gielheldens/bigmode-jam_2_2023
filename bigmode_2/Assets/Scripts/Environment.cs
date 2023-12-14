using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Environment : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private SpriteShapeController _spriteShape;
    [SerializeField] private GameObject _emptyCopy;

    [Header ("Attributes")]
    [SerializeField] private float _factor;

    [Header ("Layers")]
    [SerializeField] private string _layer;

    Vector2[] points;

    void Start()
    {
        // Set the tag and layer for the parent object
        gameObject.layer = LayerMask.NameToLayer(_layer);

        // Set the tag and layer for all children recursively
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(_layer);
        }

        // points = new Vector2[_collider.pointCount];
        // for (int i = 0; i < _collider.pointCount; i++)
        // {
        //     points[i] = _collider.points[i];
        // }
        // points[0] = UpdateFirstVertex(_collider.points[0], _collider.points[1]);
        // points[_collider.pointCount - 1] = UpdateLastVertex(_collider.points[_collider.pointCount-2], _collider.points[_collider.pointCount-1]);
        // _collider.points = points;
    }

    void Update()
    {
        //_collider.points = points;
    }

    private Vector2 UpdateFirstVertex(Vector2 p1, Vector2 p2)
    {
        Vector3 direction = p1 - p2;
        float angle = Mathf.Atan2(direction.y, direction.x);
        Vector2 _newPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _factor;
        return p1 + _newPoint;
    }

    private Vector2 UpdateLastVertex(Vector2 p2, Vector2 p1)
    {
        Vector3 direction = p1 - p2;
        float angle = Mathf.Atan2(direction.y, direction.x);
        Vector2 _newPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _factor;
        return p1 + _newPoint;
    }
}
