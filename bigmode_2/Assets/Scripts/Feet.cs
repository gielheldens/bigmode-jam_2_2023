using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{

    public bool grounded;

    private void OnTriggerStay2D(Collider2D other)
    {
        grounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        grounded = false;
    }
}
