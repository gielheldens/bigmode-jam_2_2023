using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Trigger : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private SpriteShapeRenderer _boxSprite;
    [SerializeField] private Collider2D _boxCollider;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Collider2D _myCollider;
    [SerializeField] private SpriteShapeRenderer _triggerSprite;

    [Header ("Tags")]
    [SerializeField] private string drawManagerTag;
    [SerializeField] private string staticTag;


    // private references
    private DrawManager drawManager;

    // collider variables;
    public bool isTouching;

    void Start()
    {
        drawManager = GameObject.FindWithTag(drawManagerTag).GetComponent<DrawManager>();
        _boxSprite.enabled = false;
        _boxCollider.enabled = false;
        SetColor(_triggerSprite);
        // if (transform.parent.CompareTag(staticTag)) _triggerSprite.color = drawManager.colors[0];
        // else _triggerSprite.color = drawManager.colors[1];
    }

    void Update ()
    {
        IsTouching(_layerMask);
    }

    private void IsTouching(LayerMask layers)
    {
        isTouching = Physics2D.IsTouchingLayers(_myCollider, layers);
        _boxSprite.enabled = isTouching;
        SetColor(_boxSprite);
        _boxCollider.enabled = isTouching;
    }

    private void SetColor(SpriteShapeRenderer _sprite)
    {
        if (transform.parent.CompareTag(staticTag)) _sprite.color = drawManager.colors[0];
        else _sprite.color = drawManager.colors[1];
        if (transform.parent.CompareTag("Final")) _sprite.color = drawManager.colors[2];
    }
}
