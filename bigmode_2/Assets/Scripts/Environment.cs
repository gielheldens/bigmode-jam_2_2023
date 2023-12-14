using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Environment : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private EdgeCollider2D _collider;
    [SerializeField] private SpriteShapeController _spriteShape;

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
    }
}
