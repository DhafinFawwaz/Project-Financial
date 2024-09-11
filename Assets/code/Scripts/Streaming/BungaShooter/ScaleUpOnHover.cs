using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleUpOnHover : MonoBehaviour
{
    [SerializeField] TransformAnimation _transformAnimation;
    [SerializeField] float _scaleUp = 1.2f;
    [SerializeField] UnityEvent _onMouseDown;
    [SerializeField] UnityEvent _onMouseEnter;
    [SerializeField] UnityEvent _onMouseExit;
    bool _isMouseInside = false;
    void Awake()
    {
        _transformAnimation.SetEase(Ease.OutBackCubic);
    }
    void OnMouseEnter()
    {
        _transformAnimation.SetEnd(Vector3.one * _scaleUp);
        _transformAnimation.TweenLocalScale();
        _isMouseInside = true;
        _onMouseEnter?.Invoke();
    }

    void OnMouseExit()
    {
        _transformAnimation.SetEnd(Vector3.one);
        _transformAnimation.TweenLocalScale();
        _isMouseInside = false;
        _onMouseExit?.Invoke();
    }

    void Update()
    {
        if(!_isMouseInside) return;

        if(Input.GetMouseButtonDown(0))
        {
            _onMouseDown?.Invoke();
        }
    }
}
