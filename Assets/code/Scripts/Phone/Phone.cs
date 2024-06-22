using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] AnimationUI _animationUI;
    [SerializeField] float _phoneAnimationDuration = 0.3f;
    bool _isShowing = false;

    void Update()
    {
        if(InputManager.GetKeyDown(KeyCode.Q)) Toggle();
    }


    float _lastTime = 0;
    public void Toggle()
    {
        if(Time.time - _lastTime < _phoneAnimationDuration) return;
        _lastTime = Time.time;

        _isShowing = !_isShowing;
        if(_isShowing) _animationUI.Play();
        else _animationUI.PlayReversed();
    }
}
