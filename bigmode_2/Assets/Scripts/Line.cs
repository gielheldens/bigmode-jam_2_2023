using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Line : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _colliderPrefab;

    [Header ("Attributes")]
    [SerializeField] private float _massChildFactor = 0.5f;
    [SerializeField] private float _colliderWidth;


    // draw variables
    private Vector2 _initPos;
    public List<Vector2> points = new List<Vector2>();
    private float _lineWidth;

    // for consistency, set line width to the initial line renderer width
    void Start()
    {
        _lineWidth = _lineRenderer.startWidth;
    }

    // set position of the current point on the line using pos (mouse position)
    // and a target collider
    public void SetPosition(Vector2 pos, Collider2D targetCollider)
    {
        // find closest point from a reference position (mouse position)
        // on the target collider
        Vector2 closestPos = ClosestPointOnCollider(pos, targetCollider);
        // check whether to append the new point to the line
        if(!ShouldAppend(closestPos)) return;
        // if yes, add the closest point to the list of line points
        points.Add(closestPos);
        // increase the number of points on the line renderer
        _lineRenderer.positionCount++;
        // if we drew the first point, set initial position to current position
        if(_lineRenderer.positionCount ==1) _initPos = pos;
        // set line renderer position at current index as the closest position
        _lineRenderer.SetPosition(_lineRenderer.positionCount-1, closestPos - _initPos);
    }

    // if drawing outside the target collider (in our case the draw box),
    // the line clips to the closest point on the collider bounds.
    private Vector2 ClosestPointOnCollider(Vector2 pos, Collider2D targetCollider)
    {
        Vector2 closestPoint = targetCollider.bounds.ClosestPoint(pos);
        DebugDrawCircle(pos, 0.1f, Color.red);
        return closestPoint;
    }

    // check whether we should appoint a new line point and the current mouse position,
    // this is always true if its the first line point, and else is true if the current point
    // is at least the drawing resolution width away from the previous point
    private bool ShouldAppend(Vector2 pos)
    {
        if(_lineRenderer.positionCount == 0) return true;
        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount -1), pos - _initPos) > DrawManager.RESOLUTION;
    }

    // method loops over all points in the input list and generates capsule colliders at each point
    // the method positions all colliders such that they align exactly with the lign segment
    public void GenerateColliders(List<Vector2> colliderPoints)
    {
        for (int i = 1; i < colliderPoints.Count; i++)
        {
            Vector2 pos = colliderPoints[i];
            Vector2 prevPos = colliderPoints[i-1];
            float distance = Vector2.Distance(prevPos, pos);
            float angle = AngleBetweenPoints(prevPos, pos);
            GameObject capsule = Instantiate(_colliderPrefab, pos, Quaternion.identity, transform);
            capsule.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            CapsuleCollider2D collider = capsule.GetComponent<CapsuleCollider2D>();
            collider.size = new Vector2(distance + _lineWidth, _colliderWidth);
            collider.offset = new Vector2(-(distance/2), 0f);
        }
    }

    // sets mass based on the amount of child objects on this current line prefab
    // the amount of child objects equals the amount of capsule colliders, thus making 
    // mass linearly dependent on the length of the line segment
    public void SetMass()
    {
        Rigidbody2D rb2d = transform.GetComponent<Rigidbody2D>();
        int children = transform.childCount;
        rb2d.mass = children * _massChildFactor;
    }

    // set body type of the RigidBody2D
    public void SetBodyType(RigidbodyType2D type)
    {
        Rigidbody2D rb2d = transform.GetComponent<Rigidbody2D>();
        rb2d.bodyType = type;
    }

    // set color of the line renderer
    public void SetLineColor(Color[] colors, int ind)
    {
        _lineRenderer.startColor = colors[ind];
        _lineRenderer.endColor = colors[ind];
    }
    
    private float AngleBetweenPoints(Vector2 point1, Vector2 point2)
    {
        float deltaX = point2.x - point1.x;
        float deltaY = point2.y - point1.y;
        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        return angle;
    }

    private void DebugDrawCircle(Vector2 center, float radius, Color color)
    {
        int segments = 36;
        float angleIncrement = 360f / segments;

        for (float angle = 0; angle < 360; angle += angleIncrement)
        {
            float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = center.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            Vector2 start = new Vector2(x, y);

            float nextAngle = angle + angleIncrement;
            x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * nextAngle);
            y = center.y + radius * Mathf.Sin(Mathf.Deg2Rad * nextAngle);
            Vector2 end = new Vector2(x, y);

            Debug.DrawLine(start, end, color);
        }
    }
}
