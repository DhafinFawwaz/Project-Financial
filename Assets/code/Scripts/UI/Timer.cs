using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    
    void Update()
    {
        if(!_timerGoing)return;
        _elapsedTime -= Time.unscaledDeltaTime;
        _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
        _elapsedTimeStr = _timePlaying.ToString(TimeFormat);
        _timerText.text = _prefix+_elapsedTimeStr;
        if(_elapsedTime <= 0)_timerGoing = false;
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
