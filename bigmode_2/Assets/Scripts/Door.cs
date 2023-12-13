using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    [Header ("References")]
    [SerializeField] private string _nextScene;

    [Header ("Tags")]
    [SerializeField] private string _playerTag;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            SceneManager.LoadScene(_nextScene);
        }
    }
}
