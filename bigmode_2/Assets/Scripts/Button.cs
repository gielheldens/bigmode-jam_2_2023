using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    
    [Header ("References")]
    [SerializeField] private TextMeshProUGUI _buttonTxt;
    [SerializeField] private float _hoverFontFactor;
    [SerializeField] private string _playerLayer;
    [SerializeField] private string _environmentLayer;
    [SerializeField] Image _canvasImage;


    [Header ("Tags")]
    [SerializeField] private string _uiManagerTag;
    [SerializeField] private string _levelManagerTag;

    // private references
    private UIManager _uiManager;
    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindWithTag(_uiManagerTag).GetComponent<UIManager>();
        _levelManager = GameObject.FindWithTag(_levelManagerTag).GetComponent<LevelManager>();
    }

    public void PlayLevel(string _lvl)
    {
        _levelManager.LoadNextNameLevel(_lvl);
    }

    public void EnlargeText(TextMeshProUGUI _text)
    {
        _text.fontSize *= _hoverFontFactor;
    }

    public void CompressText(TextMeshProUGUI _text)
    {
        _text.fontSize /= _hoverFontFactor;
    }

    public void SetCanvasImage(string _fileName)
    {
        Sprite sprite = Resources.Load<Sprite>(_fileName);
        _canvasImage.sprite = sprite;
    }
        
}
