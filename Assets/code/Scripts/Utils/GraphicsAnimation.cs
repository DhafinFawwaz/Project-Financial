using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsAnimation : UIAnimation
{
    [SerializeField] Graphic _target;
    public Graphic Target => _target;
    [SerializeField] Color _color = Color.white;
    [SerializeField] float _duration = 0.3f;
    [SerializeField] Ease.Function _easeFunction = Ease.OutQuart;
    Action _onceEnd;
    public Action OnceEnd { get => _onceEnd; set => _onceEnd = value; }

    [SerializeField] bool _playOnStart = false;
    void Start()
    {
        if(_playOnStart) Play();
    }

    public void Play()
    {
        StopAllOtherGraphics();
        if(!gameObject.activeInHierarchy) return;
        
        StartCoroutine(TweenColorAnimation(_target, _target.color, _color, _duration, _easeFunction));
    }

    void Reset()
    {
        _target = GetComponent<Graphic>();
    }

    public override void Stop()
    {
        _colorKey++;
    }

    public GraphicsAnimation SetEndColor(Color color)
    {
        _color = color;
        return this;
    }
    public GraphicsAnimation SetOnceEnd(Action onEnd)
    {
        _onceEnd = onEnd;
        return this;
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
        {
            g.color = end;
            _onceEnd?.Invoke();
            _onceEnd = null;
        }
    }
}