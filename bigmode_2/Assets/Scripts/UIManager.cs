using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header ("References")]
    [SerializeField] private string _lvlTut;
    [SerializeField] private string _lvl01;

    // menu variables
    public bool inMenu;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) inMenu = true;
    }

}
