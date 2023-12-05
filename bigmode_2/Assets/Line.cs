using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Line : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _colliderPrefab;


    private Vector2 _initPos;
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
        Instantiate(_colliderPrefab, pos, Quaternion.identity, transform);
    }

    private bool CanAppend(Vector2 pos)
    {
        if(_lineRenderer.positionCount == 0) return true;
        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount -1), pos - _initPos) > DrawManager.RESOLUTION;
    }
}
