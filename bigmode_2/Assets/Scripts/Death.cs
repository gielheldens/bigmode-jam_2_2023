using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string _playerTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_playerTag))
        {
            ResetCurrentScene();
        }
    }

    void ResetCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
