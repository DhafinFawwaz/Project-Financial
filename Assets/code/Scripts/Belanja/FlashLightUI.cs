using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightUI : MonoBehaviour
{
    [SerializeField] Image _rechargeBar;
    [SerializeField] TransformAnimation _animation;
    [SerializeField] Vector3 _scale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] Sprite[] _batterySprite;
    void OnEnable()
    {
        Flashlight.s_OnRecharging += OnRecharging;
    }

    void OnDisable()
    {
        Flashlight.s_OnRecharging -= OnRecharging;
    }


    Sprite _currentSprite;
    void OnRecharging(float energy)
    {
        if(_currentSprite == null) _currentSprite = _batterySprite[0];

        if(energy < 0.33f) {
            if(_currentSprite == _batterySprite[0]) return;
            _currentSprite = _batterySprite[0];
            _rechargeBar.sprite = _batterySprite[0];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        } else if(energy < 0.66f) {
            if(_currentSprite == _batterySprite[1]) return;
            _currentSprite = _batterySprite[1];
            _rechargeBar.sprite = _batterySprite[1];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        } else if(energy < 0.99f) {
            if(_currentSprite == _batterySprite[2]) return;
            _currentSprite = _batterySprite[2];
            _rechargeBar.sprite = _batterySprite[2];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        } else {
            if(_currentSprite == _batterySprite[3]) return;
            _currentSprite = _batterySprite[3];
            _rechargeBar.sprite = _batterySprite[3];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        }
    }
}
