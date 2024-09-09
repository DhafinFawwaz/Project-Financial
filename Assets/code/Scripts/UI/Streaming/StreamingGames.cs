using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public abstract class StreamingGames : MonoBehaviour
{
    public int HealthCost = 10;
    public int HappinessCost = 10;
    public abstract void Play();
    public Action<int> OnIncreaseViews;
    public Action<int> OnDecreaseViews;
    
    protected int _viewCounter = 0;
    public void SetViewCounter(int value)
    {
        _viewCounter = value;
    }

    byte _key = 0;
    protected IEnumerator TweenCanvasGroupAlphaAnimation(CanvasGroup cg, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            cg.alpha = Mathf.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key == requirement)
        {
            cg.alpha = end;
        }
    }

    public Action<StreamingGames> OnGameEnd;
    public void EndGame()
    {
        OnGameEnd?.Invoke(this);
    }

}