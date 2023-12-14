using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{

    public bool canMove;

    private void OnTriggerStay2D(Collider2D other)
    {
        canMove = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canMove = false;
    }
}
