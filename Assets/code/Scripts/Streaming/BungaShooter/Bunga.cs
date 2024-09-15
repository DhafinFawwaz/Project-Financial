using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bunga : MonoBehaviour
{
    public Action<Bunga> OnShoot;
    [SerializeField] SpriteRenderer _batang;
    [SerializeField] Transform _kepala;
    [SerializeField] float _height = 1;
    [SerializeField] float _duration = 0.3f;
    [SerializeField] ThrowAnimation _throwAnimation;
    [SerializeField] float _0PercentageHeight = 2f;
    [SerializeField] float _100PercentageHeight = 5f;

    [SerializeField] MonoBehaviour[] _toDisableOnDeath;

    [SerializeField] ShakeAnimation _shakeAnimation;
    public void Shake()
    {
        _shakeAnimation.Play();
    }

    public static Action s_OnBungaShot;
    public void Throw()
    {
        s_OnBungaShot?.Invoke();
        foreach(var obj in _toDisableOnDeath)
        {
            obj.enabled = false;
        }
        _throwAnimation.Throw();
    }

    public void DisableAll()
    {
        foreach(var obj in _toDisableOnDeath)
        {
            obj.enabled = false;
        }
    }


    public void GotShot()
    {
        OnShoot?.Invoke(this);
    }

    void OnValidate()
    {
        _batang.size = new Vector2(_batang.size.x, _height);
        _kepala.localPosition = new Vector3(_kepala.localPosition.x, -_height, _kepala.localPosition.z);
    }

    public void SetHeight(float height)
    {
        _batang.size = new Vector2(_batang.size.x, height);
        _kepala.localPosition = new Vector3(_kepala.localPosition.x, -height, _kepala.localPosition.z);
    }

    public void SetHeightAnimated(float height)
    {
        StartCoroutine(AnimateHeight(height));
    }

    byte _key;
    IEnumerator AnimateHeight(float targetHeight)
    {
        byte requirement = ++_key;
        float t = 0;
        float startHeight = _batang.size.y;

        while(t <= 1 && requirement == _key)
        {
            t += Time.deltaTime/_duration;
            float h = Mathf.Lerp(startHeight, targetHeight, Ease.OutQuart(t));
            SetHeight(h);
            yield return null;
        }
        if(requirement == _key)
        {
            SetHeight(targetHeight);
        }
    }



    int _percentage = 30;
    int _initialPercentage = 30;
    public int Percentage => _percentage;
    long _initialPrice = 100000;
    public long Price => _initialPrice;

    public long FinalPrice => (long)(_currentPrice);
    [Header("Info")]
    [SerializeField] TextMeshPro _priceLabel;
    [SerializeField] TextMeshPro _percentageLabel;
    [SerializeField] TextMeshPro _infoText;
    public void SetData(int percentage, long price)
    {
        _percentage = percentage;
        _initialPercentage = _percentage;
        _initialPrice = price;
        _currentPrice = price;

        _priceLabel.text = price.ToStringRupiahFormat();
        _percentageLabel.text = percentage.ToString() + "%";

        _infoText.text = $"{price.ToStringRupiahFormat()}";

        SetHeightAnimated(Remap(percentage, 0, 100, _0PercentageHeight, _100PercentageHeight));
    }


    long _currentPrice = 0;
    public void Increment()
    {
        long currentPriceCopy = _currentPrice;
        _currentPrice += (long)((float)_initialPrice * (float)_initialPercentage / 100f);

        // _infoText.text = $"{currentPriceCopy.ToStringRupiahFormat()} x {(_initialPercentage+100)}%\n= {_currentPrice.ToStringRupiahFormat()}";
        _infoText.text = $"{_currentPrice.ToStringRupiahFormat()}";

        
        _percentage += 10;
        SetHeightAnimated(Remap(_percentage, 0, 100, _0PercentageHeight, _100PercentageHeight));

    }


    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
