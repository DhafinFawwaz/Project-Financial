using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject _blocker;
    [SerializeField] RectTransform _rt;
    [SerializeField] CanvasGroup _canvasGroup;
    Action onDone;
    public bool IsVisible => _blocker.activeSelf;
    [SerializeField] bool _isShowing = false;
    public void Show()
    {
        if(_isShowing) return;
        _isShowing = true;

        _blocker.SetActive(true);
        StartCoroutine(TweenLocalScaleAnimation(_rt, _rt.localScale, Vector3.one, 0.3f, Ease.OutQuart));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 0, 1, 0.3f, Ease.OutQuart));
    }

    public void Hide()
    {
        if(!_isShowing) return;
        _isShowing = false;
        Debug.Log("hide");

        onDone += () => _blocker.SetActive(false);
        StartCoroutine(TweenLocalScaleAnimation(_rt, _rt.localScale, Vector3.zero, 0.2f, Ease.InCubic));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 1, 0, 0.2f, Ease.InCubic));
        InputManager.SetActiveMouseAndKey(true);
    }
    byte _key = 0;
    IEnumerator TweenLocalScaleAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key == requirement)
        {
            rt.localScale = end;
            onDone?.Invoke();
            onDone = null;
        }
    }

    byte _key2 = 0;
    IEnumerator TweenCanvasGroupAlphaAnimation(CanvasGroup cg, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key2;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key2 == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            cg.alpha = Mathf.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key2 == requirement)
        {
            cg.alpha = end;
            onDone?.Invoke();
            onDone = null;
        }
    }
}
