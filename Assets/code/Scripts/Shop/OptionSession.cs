using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionSession : MonoBehaviour
{
    [SerializeField] GraphicsAnimation _blackAnimation;
    [SerializeField] ItemThrower _itemThrower;
    [SerializeField] Option _optionPrefab;
    [SerializeField] Transform _optionContainer;
    [SerializeField] Transform _eToSelesai;
    public UnityEvent<OptionSession> OnOptionChoosen;
    [SerializeField] Timer _timeLimit;
    List<Option> _activeOptions = new List<Option>();
    Rak _currentRak;
    public OptionData OptionData => _currentRak.OptionData;
    [SerializeField] UnityEvent _onOptionStart;
    [SerializeField] UnityEvent _onOptionEnd;
    
    public void SetDataAndPlay(Rak rak)
    {
        _onOptionStart?.Invoke();
        _blackAnimation.SetEndColor(_activeBackgroundColor).Play();
        _openTime = Time.time;
        _isOpen = true;
        _currentRak = rak;
        _currentRak.ReGacha();
        this.Invoke(_vignetteEffect.ResetLevel, 0.1f);

        // Populate
        OptionData optionData = rak.OptionData;
        for (int i = 0; i < optionData.Content.Length; i++)
        {
            Option option = Instantiate(_optionPrefab, _optionContainer);
            option.SetValues(optionData.Content[i]);
            option.SetSprite(optionData.ItemSprite);
            _activeOptions.Add(option);
            option.transform.localScale = Vector3.zero;
        }
        StartCoroutine(ScalingAnimation());

        for (int i = 0; i < optionData.Content.Length; i++)
        {
            int I = i;
            _activeOptions[i].GetComponent<ButtonUI>().OnClick.AddListener(() =>
            {
                for(int j = 0; j < _activeOptions.Count; j++)
                {
                    if(j != I) _activeOptions[j].ResetBuyCount();
                }
                var option = _activeOptions[I].GetComponent<Option>();
                option.IncrementBuyCount();
                // if(WorldUI.CurrentBelanjaMoney >= option.Content.Price * (option.BuyCount+1)) {
                // }
            });
        }

        StartCoroutine(TweenLocalScaleAnimation(_eToSelesai as RectTransform, _eToSelesai.localScale, Vector3.one, 0.2f, Ease.OutBackCubic));

        // Check if shadow kena senter, if yes, set the below to 16
        _vignetteEffect.gameObject.SetActive(true);
        if(_useTimer) StartCoroutine(TimerCountdown(_currentRak.SessionTime));
    }

    [SerializeField] bool _useTimer = true;


    [SerializeField] VignetteEffect _vignetteEffect;
    byte _timerCountdownKey = 0;
    [SerializeField] UnityEvent OnKerasukan;
    [SerializeField] float _darkenTime = 4;
    IEnumerator TimerCountdown(float time)
    {
        _vignetteEffect.gameObject.SetActive(true);
        _timeLimit.SetTime(time);
        _timeLimit.Begin();
        byte requirement = ++_timerCountdownKey;


        // yield return new WaitForSeconds(time-_darkenTime);
        // for(int i = 0; i < _darkenTime; i++)
        // {
        //     if(_timerCountdownKey != requirement) break;
        //     _vignetteEffect.IncreaseLevel();
        //     yield return new WaitForSeconds(1);
        // }

        // if(_timerCountdownKey == requirement)
        // {
        //     // kerasukan
        //     OnKerasukan?.Invoke();
        //     _currentRak.SetDarken(true);
        //     Close();
            
        //     if(PlayerCore.Instance != null)
        //         PlayerCore.Instance.MoveCameraBack();
        // }

        while(_timeLimit.ElapsedTime > 0 && _timerCountdownKey == requirement)
        {
            yield return null;

            if(_timeLimit.ElapsedTime > 3 && _timeLimit.ElapsedTime <= 4)
            {
                _vignetteEffect.SetLevel(0);
            }
            else if(_timeLimit.ElapsedTime > 2 && _timeLimit.ElapsedTime <= 3)
            {
                _vignetteEffect.SetLevel(1);
            }
            else if(_timeLimit.ElapsedTime > 1 && _timeLimit.ElapsedTime <= 2)
            {
                _vignetteEffect.SetLevel(2);
            }
            else if(_timeLimit.ElapsedTime > 0 && _timeLimit.ElapsedTime <= 1)
            {
                _vignetteEffect.SetLevel(3);
            }
        }
        if(_timerCountdownKey == requirement)
        {
            // kerasukan
            OnKerasukan?.Invoke();
            _currentRak.SetDarken(true);
            Close();
            
            if(PlayerCore.Instance != null)
                PlayerCore.Instance.MoveCameraBack();
        }

        // 0 4
        // 1 3
        // 2 2
        // 3 1
    }
    void CancelTimerCountdown()
    {
        _timerCountdownKey++;
        _vignetteEffect.ResetLevel();
        this.Invoke(_vignetteEffect.ResetLevel, 0.1f);
    }

    IEnumerator ScalingAnimation()
    {
        RectTransformAnimation rta = _timeLimit.GetComponent<RectTransformAnimation>();
        rta.SetEnd(Vector2.one).TweenLocalScale();

        for (int i = 0; i < _activeOptions.Count; i++)
        {
            _activeOptions[i].Show();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Close()
    {
        _currentRak.SessionTime = _timeLimit.ElapsedTime;
        _onOptionEnd?.Invoke();
        for (int i = 0; i < _activeOptions.Count; i++)
            _activeOptions[i].Hide();
        _activeOptions.Clear();

        
        RectTransformAnimation rta = _timeLimit.GetComponent<RectTransformAnimation>();
        rta.SetEnd(Vector2.zero).TweenLocalScale();
        StartCoroutine(TweenLocalScaleAnimation(_eToSelesai as RectTransform, _eToSelesai.localScale, Vector3.zero, 0.2f, Ease.OutQuart));
        CancelTimerCountdown();
        _isOpen = false;
        _blackAnimation.SetEndColor(_inactiveBackgroundColor).Play();
    }
    

    public Option GetChoosenOption()
    {
        for (int i = 0; i < _activeOptions.Count; i++)
        {
            if(_activeOptions[i].BuyCount > 0)
            {
                return _activeOptions[i];
            }
        }
        return null;
    }

    float _openTime;


    bool _isOpen = false;
    public void ChooseOption()
    {
        if(!_isOpen) return;
        if(!(Time.time - _openTime > 0.5f)) return;

        if(GetChoosenOption() == null) {
            Close();
            if(PlayerCore.Instance != null)
                PlayerCore.Instance.MoveCameraBack();
            StartCoroutine(DisableCollected(_currentRak));
            return; // case not buying anything
        }

        OnOptionChoosen?.Invoke(this);
        Close();
        if(PlayerCore.Instance != null)
            PlayerCore.Instance.MoveCameraBack();
        StartCoroutine(DisableCollected(_currentRak));

        _itemThrower.Buy(this);
    }

    IEnumerator DisableCollected(Rak rak)
    {
        yield return null;
        rak.IsCollected = false;
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


    
    [Header("Background")]
    [SerializeField] Color _activeBackgroundColor = new Color(0,0,0,0.7f);
    [SerializeField] Color _inactiveBackgroundColor = Color.clear;
}
