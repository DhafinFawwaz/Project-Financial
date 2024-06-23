using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformAnimation : MonoBehaviour
{
    [SerializeField] RectTransform _target;
    [SerializeField] Vector3 _end;
    [SerializeField] float _duration;
    Ease.Function _easeFunction = Ease.OutQuart;
    [SerializeField] Ease.Type _easeType = Ease.Type.Out;
    [SerializeField] Ease.Power _easePower = Ease.Power.Quart;
    [SerializeField] RectTransformAnimation[] _animationToStop;
    void Start()
    {
        _easeFunction = Ease.GetEase(_easeType, _easePower);
    }

    public void Stop()
    {
        _scaleKey++;
        _posKey++;
        _rotKey++;
    }

    public RectTransformAnimation SetEnd(Vector3 end)
    {
        _end = end;
        return this;
    }

    public void TweenLocalScale()
    {
        foreach (var a in _animationToStop) a.Stop();
        StartCoroutine(TweenLocalScaleAnimation(_target, _target.localScale, _end, _duration, _easeFunction));
    }

    public void TweenEulerAngles()
    {
        foreach (var a in _animationToStop) a.Stop();
        StartCoroutine(TweenEulerAnglesAnimation(_target, _target.localEulerAngles, _end, _duration, _easeFunction));
    }

    public void TweenPosition()
    {
        foreach (var a in _animationToStop) a.Stop();
        StartCoroutine(TweenPositionAnimation(_target, _target.localPosition, _end, _duration, _easeFunction));
    }



    byte _rotKey = 0;
    IEnumerator TweenEulerAnglesAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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
    IEnumerator TweenPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_posKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _posKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_posKey == requirement)
            rt.localPosition = end;
    }


    byte _scaleKey = 0;
    IEnumerator TweenLocalScaleAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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
