using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] string _prefix = "TIME : ";
    TimeSpan _timePlaying;

    bool _timerGoing;
    string _elapsedTimeStr;
    
    public float ElapsedTime{get{return _elapsedTime;}}
    public string ElapsedTimeStr{get{return _elapsedTimeStr;}}
    [ReadOnly] float _elapsedTime;
    // public const string TimeFormat = @"hh\:mm\:ss";
    // minute and seconds only
    public const string TimeFormat = @"mm\:ss";

    [SerializeField] bool _useUnscaledTime = true;
    [SerializeField] UnityEvent _onTimeEnd;
    void Update()
    {
        if(!_timerGoing)return;
        _elapsedTime -= _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
        _elapsedTimeStr = _timePlaying.ToString(TimeFormat);
        _timerText.text = _prefix+_elapsedTimeStr;
        if(_elapsedTime <= 0)
        {
            _timerGoing = false;
            _onTimeEnd?.Invoke();
        }
    }


    public void Pause()
    {
        _timerGoing = false;
    }

    public void Resume()
    {
        _timerGoing = true;
    }

    
    public void Begin()
    {
        _timerGoing = true;
    }
    public void End()
    {
        _timerGoing = false;
    }

    public void SetTime(float time)
    {
        _elapsedTime = time;
    }
}
