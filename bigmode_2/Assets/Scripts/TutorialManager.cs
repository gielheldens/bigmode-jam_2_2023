using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private GameObject _triggerCanvas;
    [SerializeField] private GameObject _boxCanvas;
    [SerializeField] private GameObject _doorCanvas;

    // private ref rences
    private Trigger _trigger;
    private DrawManager _drawManager;

    private bool _once = true;

    void Start()
    {
        _trigger = GameObject.FindWithTag("Trigger").GetComponent<Trigger>();
        _drawManager = GameObject.FindWithTag("DrawManager").GetComponent<DrawManager>();
    }



    // Update is called once per frame
    void Update()
    {   
        if(Input.GetAxisRaw("Horizontal") > 0) _triggerCanvas.SetActive(true);
        if(_trigger.isTouching && _once) 
        {
            _once = false;
            _boxCanvas.SetActive(true);
            _triggerCanvas.SetActive(false);
        }
        if(_drawManager.drawing) 
        {
            _doorCanvas.SetActive(true);
            _boxCanvas.SetActive(false);}

    }
}
