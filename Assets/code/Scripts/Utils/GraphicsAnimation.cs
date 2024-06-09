using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsAnimation : MonoBehaviour
{
    [SerializeField] Graphic _target;
    [SerializeField] Color _color = Color.white;
    [SerializeField] float _duration = 0.3f;
    [SerializeField] Ease.Function _easeFunction = Ease.OutQuart;
    [SerializeField] GraphicsAnimation[] _graphicToStop;

    public void Play()
    {
        foreach (var g in _graphicToStop)
            g.Stop();
        StartCoroutine(TweenColorAnimation(_target, _target.color, _color, _duration, _easeFunction));
    }

    public void Stop()
    {
        _colorKey++;
    }
    
    byte _colorKey = 0;
    IEnumerator TweenColorAnimation(Graphic g, Color start, Color end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_colorKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _colorKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            g.color = Color.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_colorKey == requirement)
            g.color = end;
    }
}