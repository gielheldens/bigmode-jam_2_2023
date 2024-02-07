using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    [Header ("Tags")]
    [SerializeField] private string _lineTag;
    [SerializeField] private string _environmentTag;

    public bool grounded;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(_lineTag) || other.CompareTag(_environmentTag))
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_lineTag) || other.CompareTag(_environmentTag))
        {
            grounded = false;
        }
    }
}
