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
        Flashlight.s_OnNotEnoughEnergy += s_OnNotEnoughEnergy;
        initPos = _animation.transform.parent.position;
    }
    Vector3 initPos;

    void OnDisable()
    {
        Flashlight.s_OnRecharging -= OnRecharging;
        Flashlight.s_OnNotEnoughEnergy -= s_OnNotEnoughEnergy;
    }

    void s_OnNotEnoughEnergy()
    {
        StartCoroutine(Shake());
    }

    [Header("Shake")]
    [SerializeField] float _shakeAmplitude = 100;
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeFrequency = 50f;

    byte _shakeKey;
    IEnumerator Shake()
    {
        byte requirement = ++_shakeKey;
        float t = 0;
        while(t <= 1 && requirement == _shakeKey)
        {
            t += Time.deltaTime / _shakeDuration;
            float x = t * _shakeFrequency;
            float y = (1-t) * _shakeAmplitude * Mathf.Sin(x*_shakeFrequency);
            _animation.transform.parent.position = initPos + new Vector3(y, 0, 0);
            yield return null;
        }
        if(requirement == _shakeKey) _animation.transform.parent.position = initPos;
    }


    Sprite _currentSprite;
    public static System.Action<float> s_OnRechargingUI;
    void OnRecharging(float energy)
    {
        if(_currentSprite == null) _currentSprite = _batterySprite[0];

        if(energy < 0.33f) {
            if(_currentSprite == _batterySprite[0]) return;
            _currentSprite = _batterySprite[0];
            _rechargeBar.sprite = _batterySprite[0];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
            // s_OnRechargingUI?.Invoke(energy);
        } else if(energy < 0.66f) {
            if(_currentSprite == _batterySprite[1]) return;
            if(_currentSprite == _batterySprite[0])
                s_OnRechargingUI?.Invoke(energy);

            _currentSprite = _batterySprite[1];
            _rechargeBar.sprite = _batterySprite[1];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        } else if(energy < 0.99f) {
            if(_currentSprite == _batterySprite[2]) return;
            if(_currentSprite == _batterySprite[1])
                s_OnRechargingUI?.Invoke(energy);

            _currentSprite = _batterySprite[2];
            _rechargeBar.sprite = _batterySprite[2];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
        } else {
            if(_currentSprite == _batterySprite[3]) return;
            if(_currentSprite == _batterySprite[2])
                s_OnRechargingUI?.Invoke(energy);

            _currentSprite = _batterySprite[3];
            _rechargeBar.sprite = _batterySprite[3];
            _animation.transform.localScale = _scale;
            _animation.TweenLocalScale();
            s_OnRechargingUI?.Invoke(energy);
        }
    }
}
