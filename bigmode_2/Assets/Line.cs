using System.Collections;
using System.Collections.Generic;
//using System.Numerics;

//using System.Numerics;
using UnityEngine;

public class Line : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _colliderPrefab;

    [Header ("Attributes")]
    [SerializeField] private float _lineWidth = 0.2f;
    [SerializeField] private float _massChildFactor = 0.5f;


    // draw variables
    private Vector2 _initPos;
    public List<Vector2> points = new List<Vector2>();

    void Start()
    {
        _lineWidth = _lineRenderer.startWidth;
    }

    public void SetPosition(Vector2 pos, Collider2D targetCollider)
    {
        Vector2 closestPos = ColliderCheck(pos, targetCollider);
        if(!ShouldAppend(closestPos)) return;
        points.Add(closestPos);
        _lineRenderer.positionCount++;
        if(_lineRenderer.positionCount ==1) _initPos = pos;
        _lineRenderer.SetPosition(_lineRenderer.positionCount-1, closestPos - _initPos);
    }

    private bool ShouldAppend(Vector2 pos)
    {
        if(_lineRenderer.positionCount == 0) return true;
        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount -1), pos - _initPos) > DrawManager.RESOLUTION;
    }

    private Vector2 ColliderCheck(Vector2 pos, Collider2D targetCollider)
    {
        Vector2 closestPoint = targetCollider.bounds.ClosestPoint(pos);
        DebugDrawCircle(pos, 0.1f, Color.red);
        return closestPoint;
    }

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
            collider.size = new Vector2(distance + _lineWidth, _lineWidth);
            collider.offset = new Vector2(-(distance/2), 0f);
        }
    }

    public void SetMass()
    {
        Rigidbody2D rb2d = transform.GetComponent<Rigidbody2D>();
        int children = transform.childCount;
        rb2d.mass = children * _massChildFactor;
    }

    public void SetBodyType(RigidbodyType2D type)
    {
        Rigidbody2D rb2d = transform.GetComponent<Rigidbody2D>();
        rb2d.bodyType = type;
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
