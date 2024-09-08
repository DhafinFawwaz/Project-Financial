using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiscalGuardianGuide : MonoBehaviour
{
    bool _isPlaying = false;
    public void SetIspLaying(bool isPlaying)
    {
        _isPlaying = isPlaying;
    }

    [SerializeField] AnimationUI _animationUI;

    float _lastTime = 0;
    [SerializeField] float _noInteractionTime = 5;

    public void UpdateInteraction()
    {
        _lastTime = Time.time;
    }

    void Update()
    {
        if(!_isPlaying) return;
        if(Time.time - _lastTime > _noInteractionTime)
        {
            _animationUI.PreviewStart();
            _animationUI.Play();
            _lastTime = Time.time - 2;
        }
    }
}
