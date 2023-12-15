using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    [Header ("References")]
    [SerializeField] private string _nextScene;

    [Header ("Tags")]
    [SerializeField] private string _playerTag;
    [SerializeField] private string _levelManagerTag;

    [Header ("Sounds")]
    [SerializeField] private AudioSource _nextLvlSound;

    // private references
    private LevelManager _levelManager;

    void Start()
    {
        _levelManager = GameObject.FindWithTag(_levelManagerTag).GetComponent<LevelManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            //SceneManager.LoadScene(_nextScene);
            _nextLvlSound.Play();
            _levelManager.LoadNextLevel();
        }
    }
}
