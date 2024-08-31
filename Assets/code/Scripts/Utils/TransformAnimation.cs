using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformAnimation : UIAnimation
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _end;
    [SerializeField] float _duration;
    [SerializeField] Ease.Function _easeFunction = Ease.OutQuart;

    public void SetEase(Ease.Function easeFunction)
    {
        _easeFunction = easeFunction;
    }

    public void TweenLocalScale()
    {
        StopAllOtherGraphics();
        StartCoroutine(TweenLocalScaleAnimation(_target, _target.localScale, _end, _duration, _easeFunction));
    }

    public void TweenEulerAngles()
    {
        StopAllOtherGraphics();
        StartCoroutine(TweenEulerAnglesAnimation(_target, _target.localEulerAngles, _end, _duration, _easeFunction));
    }

    public void TweenPosition()
    {
        StopAllOtherGraphics();
        StartCoroutine(TweenPositionAnimation(_target, _target.localPosition, _end, _duration, _easeFunction));
    }

    public override void Stop()
    {
        _scaleKey++;
        _rotKey++;
        _posKey++;
    }

    byte _rotKey = 0;
    IEnumerator TweenEulerAnglesAnimation(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_rotKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _rotKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localEulerAngles = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_rotKey == requirement)
            rt.localEulerAngles = end;
    }


    byte _posKey = 0;
    IEnumerator TweenPositionAnimation(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_posKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _posKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_posKey == requirement)
            rt.position = end;
    }


    byte _scaleKey = 0;
    IEnumerator TweenLocalScaleAnimation(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_scaleKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _scaleKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_scaleKey == requirement)
            rt.localScale = end;
    }

}
