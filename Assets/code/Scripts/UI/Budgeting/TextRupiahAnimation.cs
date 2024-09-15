using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRupiahAnimation : TextAnimation
{
    [SerializeField] float _initialValue = 1000;
    [SerializeField] float _finalValue = 0;
    [SerializeField] float _duration = 1;

    public static System.Action s_OnRupiahAnimationStart;

    public void Play()
    {
        s_OnRupiahAnimationStart?.Invoke();
        SetAndAnimate(_initialValue, _finalValue, _duration);
    }

    public void SetValues(float initialValue, float finalValue)
    {
        _initialValue = initialValue;
        _finalValue = finalValue;
        _text.text = _prefix + _initialValue.ToStringRupiahFormat();
    }

    protected override string getFormattedValue(float value)
    {
        return _prefix + value.ToStringRupiahFormat();
    }

    public override void SetPrefix(string prefix)
    {
        _prefix = prefix;
        _text.text = _prefix;
    }
}
