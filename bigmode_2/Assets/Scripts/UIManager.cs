using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    //[Header ("References")]

    // menu variables
    public bool inMenu;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) inMenu = true;
    }

}
