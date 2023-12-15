using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Animator _crossfade;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _canvasImage;
    [SerializeField] private string _mainMenuScene;
    [SerializeField] private string _BG;
    [SerializeField] private string _lvl01Scene;
    [SerializeField] private string _lvl01BG;
    [SerializeField] private string _tutScene;
    [SerializeField] private string _tutBG;

    [Header ("Attributes")]
    [SerializeField] private float _fadeOutTime;


    // private variables
    public string _prevScene ="DefaultSceneName";
    private string _prevSceneKey = "PreviousScene";

    private float _fadeTimer;
    private bool _changedBG;
    
    void Start()
    {
        _canvasGroup.alpha = 1;
        _prevScene = PlayerPrefs.GetString(_prevSceneKey, "DefaultSceneName");
        if(_prevScene == _mainMenuScene && SceneManager.GetActiveScene().name == _tutScene)
        {
            Sprite sprite = Resources.Load<Sprite>(_tutBG);
            _canvasImage.sprite = sprite;
            _changedBG = true;
        }

        if(_prevScene == _mainMenuScene && SceneManager.GetActiveScene().name == _lvl01Scene)
        {
            Sprite sprite = Resources.Load<Sprite>(_lvl01BG);
            _canvasImage.sprite = sprite;
            _changedBG = true;
        }
    }
    
    void Update()
    {
        if (_fadeTimer < _fadeOutTime && _changedBG) _fadeTimer += Time.deltaTime;
        if (_fadeTimer > _fadeOutTime) 
        {
            _fadeTimer = 0;
            Sprite sprite = Resources.Load<Sprite>(_BG);
            _canvasImage.sprite = sprite;
            _changedBG = false;
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadNextNameLevel(string _name)
    {
        StartCoroutine(LoadLevelName(_name));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //string _prevScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(_prevSceneKey, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        _crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(_fadeOutTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelName(string _name)
    {
        //string _prevScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(_prevSceneKey, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        _crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(_fadeOutTime);

        SceneManager.LoadScene(_name);
    }
}
