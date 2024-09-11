using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGuide : MonoBehaviour
{
    [SerializeField] float _noMovemetTime = 2f;
    [SerializeField] GraphicsAnimation _graphicsAnimation;

    float _lastTimeMoved = 0;
    bool _isShowing = false;
    void Update()
    {
        PlayerCore _core = PlayerCore.Instance;
        if (_core == null) return;


        if(_core.IsMoving())
        {
            _lastTimeMoved = Time.time;
            if(_isShowing)
            {
                _graphicsAnimation.SetEndColor(new Color(1, 1, 1, 0)).Play();
                _graphicsAnimation.SetOnceEnd(() => {
                    _graphicsAnimation.Target.gameObject.SetActive(false);
                });
                _isShowing = false;
            }
            return;
        }



        if(Time.time - _lastTimeMoved > _noMovemetTime && !_isShowing)
        {
            _graphicsAnimation.Target.gameObject.SetActive(true);
            _graphicsAnimation.SetEndColor(Color.white).Play();
            _isShowing = true;
        }
        
    }
}
