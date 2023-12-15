using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Animator _crossfade;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _tutGroup;
    [SerializeField] private CanvasGroup _lvl01Group;

    [Header ("Attributes")]
    [SerializeField] private float _fadeOutTime;
    
    void Start()
    {
        _canvasGroup.alpha = 1;
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
        _crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(_fadeOutTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelName(string _name)
    {
        _crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(_fadeOutTime);

        SceneManager.LoadScene(_name);
    }
}
