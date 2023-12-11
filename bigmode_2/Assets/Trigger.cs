using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Trigger : MonoBehaviour
{
    //[Header ("References")]
    [SerializeField] private string drawManagerTag;
    [SerializeField] private SpriteShapeRenderer _boxSprite;
    [SerializeField] private Collider2D _boxCollider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider2D _myCollider;

    // collider variables;
    private bool isTouching;


    void Update ()
    {
        IsTouching(_layerMask);
    }

    private void IsTouching(LayerMask layers)
    {
        isTouching = Physics2D.IsTouchingLayers(_myCollider, layers);
        _boxSprite.enabled = isTouching;
        _boxCollider.enabled = isTouching;
    }
}
