using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlicedFilledImageFillAnimation : UIAnimation
{
    [SerializeField] SlicedFilledImage _target;
    public SlicedFilledImage Target => _target;
    [SerializeField] float _fill = 1;
    [SerializeField] float _duration = 0.3f;
    public void SetDuration(float duration)
    {
        _duration = duration;
    }
    [SerializeField] Ease.Function _easeFunction = Ease.OutQuart;
    Action _onEnd;

    public void Play()
    {
        StopAllOtherGraphics();
        StartCoroutine(TweenFillAnimation(_target, _target.fillAmount, _fill, _duration, _easeFunction));
    }

    public override void Stop()
    {
        _fillKey++;
    }

    public SlicedFilledImageFillAnimation SetFill(float fill)
    {
        _target.fillAmount = fill;
        return this;
    }

    public SlicedFilledImageFillAnimation SetEndFill(float fill)
    {
        _fill = fill;
        return this;
    }
    public SlicedFilledImageFillAnimation SetOnceEnd(Action onEnd)
    {
        _onEnd = onEnd;
        return this;
    }

    public SlicedFilledImageFillAnimation SetEaseFunction(Ease.Function easeFunction)
    {
        _easeFunction = easeFunction;
        return this;
    }
    
    byte _fillKey = 0;
    IEnumerator TweenFillAnimation(SlicedFilledImage img, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_fillKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _fillKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            img.fillAmount = Mathf.LerpUnclamped(start, end, easeFunction(t));
            
            yield return null;
        }
        if(_fillKey == requirement)
        {
            img.fillAmount = end;
            _onEnd?.Invoke();
            _onEnd = null;
        }
    }
}