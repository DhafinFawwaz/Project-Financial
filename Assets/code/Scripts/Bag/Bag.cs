using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] AnimationUI _animationUI;
    [SerializeField] float _bagAnimationDuration = 0.3f;
    bool _isShowing = false;

    float _lastTime = 0;

    // Called by hovering over the bag at the bottom right of the screen
    public void Toggle()
    {
        if(Time.time - _lastTime < _bagAnimationDuration) return;
        _lastTime = Time.time;

        _isShowing = !_isShowing;
        if(_isShowing) _animationUI.Play();
        else _animationUI.PlayReversed();
    }
}
