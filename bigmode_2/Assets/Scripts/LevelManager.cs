using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Animator _crossfade;
    [SerializeField] private CanvasGroup _canvasGroup;

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

    IEnumerator LoadLevel(int levelIndex)
    {
        _crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(_fadeOutTime);

        SceneManager.LoadScene(levelIndex);
    }
}
