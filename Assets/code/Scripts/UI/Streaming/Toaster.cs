using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Toaster : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textTitle;
    [SerializeField] TextMeshProUGUI _textMessage;

    public void SetText(string title, string message)
    {
        _textTitle.text = title;
        _textMessage.text = message;
    }

    public void Move(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction, Action OnComplete = null)
    {
        StartCoroutine(TweenAnchoredPositionAnimation(rt, start, end, duration, easeFunction, OnComplete));
    }

    byte _key2;
    IEnumerator TweenAnchoredPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction, Action OnComplete = null)
    {
        byte requirement = ++_key2;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _key2)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(requirement == _key2){
            rt.anchoredPosition = end;
            OnComplete?.Invoke();
        }
    }
}
