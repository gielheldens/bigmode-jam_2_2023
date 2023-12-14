using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    [Header ("Tags")]
    [SerializeField] private string _groundTag;

    public bool grounded;

    private void OnTriggerStay2D(Collider2D other)
    {
        grounded = true;
        // if (other.CompareTag(_groundTag))
        // {
        //     grounded = true;
        // }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        grounded = false;
        // if (other.CompareTag(_groundTag))
        // {
        //     grounded = false;
        // }
    }
}
