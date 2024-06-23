using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamerGame : MonoBehaviour
{
    RectTransform _rt;
    void Start()
    {
        _rt = GetComponent<RectTransform>();
    }
    public void Hightlight()
    {
        StartCoroutine(TweenLocalScaleAnimation(_rt, _rt.localScale, Vector3.one * 1.2f, 0.25f, Ease.OutBackCubic));
    }

    public void Unhightlight()
    {
        StartCoroutine(TweenLocalScaleAnimation(_rt, _rt.localScale, Vector3.one, 0.25f, Ease.OutQuart));
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
