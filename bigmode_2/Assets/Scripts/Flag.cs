using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{

    [Header ("References")]
    [SerializeField] private Collider2D _collider;
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
