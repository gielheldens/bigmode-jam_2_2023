using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [Header ("Layers")]
    [SerializeField] private string _layer;

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
