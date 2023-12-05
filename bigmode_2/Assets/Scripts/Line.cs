using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private LineRenderer _renderer;
    
    
    private EdgeCollider2D _collider;

    public List<Vector2> _points = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        _collider = transform.parent.GetComponent<EdgeCollider2D>();
        //transform.position += transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetPosition(Vector2 pos) 
    {
        if(!CanAppend(pos)) return;

        _points.Add(pos);
        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount-1, pos);
  
        
    }

    public void SetCollider(List<Vector2> _)
    {
        _collider.points = _.ToArray();
    }

    private bool CanAppend(Vector2 pos)
    {
        if(_renderer.positionCount == 0) return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount-1),pos) > DrawManager.RESOLUTION;
    }

    // public void UpdatePosition(int i, Vector2 pos)
    // {
    //     _renderer.SetPosition(i, pos);
    // }
}
