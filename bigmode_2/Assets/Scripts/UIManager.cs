using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    //[Header ("References")]
    // private references
    private GameObject _pauseMenu;
    private GameObject _overlay;
    private LevelManager _levelManager;

    // menu variables
    public bool inMenu;
    public static bool isPaused;

    void Awake()
    {
        _pauseMenu = GameObject.FindWithTag("PauseMenu");
        _pauseMenu.SetActive(false);
        _overlay = GameObject.FindWithTag("Overlay");
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) inMenu = true;
    }

    void Update()
    {
        if(inMenu) _overlay.SetActive(false);

        if(!inMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _overlay.SetActive(true);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        _overlay.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
    }

}
