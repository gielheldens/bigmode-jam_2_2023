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
    [SerializeField] private float _lineWidth = 0.1f;


    private Vector2 _initPos;
    private Vector2 _prevPos;
    public List<Vector2> points = new List<Vector2>();

    void Start()
    {
        
    }

    void Update()
    {

        
    }

    public void SetPosition(Vector2 pos)
    {
        if(!CanAppend(pos)) return;

        _lineRenderer.positionCount++;
        if(_lineRenderer.positionCount ==1) _initPos = pos;
        _lineRenderer.SetPosition(_lineRenderer.positionCount-1, pos - _initPos);

        if(_lineRenderer.positionCount >= 2) GenerateCollider(pos);
        
        _prevPos = pos;
    }

    private bool CanAppend(Vector2 pos)
    {
        if(_lineRenderer.positionCount == 0) return true;
        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount -1), pos - _initPos) > DrawManager.RESOLUTION;
    }

    private void GenerateCollider(Vector2 pos)
    {
        float distance = Vector2.Distance(_prevPos, pos);
        float angle = AngleBetweenPoints(_prevPos, pos);
        GameObject capsule = Instantiate(_colliderPrefab, pos, Quaternion.identity, transform);
        capsule.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        CapsuleCollider2D collider = capsule.GetComponent<CapsuleCollider2D>();
        collider.size = new Vector2(distance + _lineWidth, _lineWidth);
        collider.offset = new Vector2(-(distance/2), 0f);
    }

    private float AngleBetweenPoints(Vector2 point1, Vector2 point2)
    {
        float deltaX = point2.x - point1.x;
        float deltaY = point2.y - point1.y;
        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        return angle;
    }
}
