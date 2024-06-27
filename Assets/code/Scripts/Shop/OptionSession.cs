using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionSession : MonoBehaviour
{
    [SerializeField] ItemThrower _itemThrower;
    [SerializeField] Option _optionPrefab;
    [SerializeField] Transform _optionContainer;
    [SerializeField] Transform _eToSelesai;
    [SerializeField] UnityEvent<OptionSession> OnOptionChoosen;
    [SerializeField] Timer _timeLimit;
    List<Option> _activeOptions = new List<Option>();
    bool _isPlaying = false;    
    Rak _currentRak;
    public OptionData OptionData => _currentRak.OptionData;
    public void SetDataAndPlay(Rak rak)
    {
        _isPlaying = true;
        _openTime = Time.time;
        _currentRak = rak;
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
            });
        }

        StartCoroutine(TweenLocalScaleAnimation(_eToSelesai as RectTransform, _eToSelesai.localScale, Vector3.one, 0.2f, Ease.OutBackCubic));

        // Check if shadow kena senter, if yes, set the below to 16
        _vignetteEffect.gameObject.SetActive(true);
        StartCoroutine(TimerCountdown(10));
    }


    [SerializeField] VignetteEffect _vignetteEffect;
    byte _timerCountdownKey = 0;
    [SerializeField] UnityEvent OnKerasukan;
    IEnumerator TimerCountdown(float time)
    {
        _vignetteEffect.gameObject.SetActive(true);
        _timeLimit.SetTime(time);
        _timeLimit.Begin();
        byte requirement = ++_timerCountdownKey;


        yield return new WaitForSeconds(time-5);
        for(int i = 0; i < 5; i++)
        {
            if(_timerCountdownKey != requirement) break;
            _vignetteEffect.IncreaseLevel();
            yield return new WaitForSeconds(1);
        }

        if(_timerCountdownKey == requirement)
        {
            // kerasukan
            OnKerasukan?.Invoke();
            Close();
            
            if(PlayerCore.Instance != null)
                PlayerCore.Instance.MoveCameraBack();
        }
    }
    void CancelTimerCountdown()
    {
        _vignetteEffect.ResetLevel();
        _vignetteEffect.gameObject.SetActive(false);
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
        for (int i = 0; i < _activeOptions.Count; i++)
            _activeOptions[i].Hide();
        _activeOptions.Clear();

        
        RectTransformAnimation rta = _timeLimit.GetComponent<RectTransformAnimation>();
        rta.SetEnd(Vector2.zero).TweenLocalScale();
        StartCoroutine(TweenLocalScaleAnimation(_eToSelesai as RectTransform, _eToSelesai.localScale, Vector3.zero, 0.2f, Ease.OutQuart));
        CancelTimerCountdown();
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
    void Update()
    {
        if(!_isPlaying) return;
        if(Input.GetKeyDown(KeyCode.E) && Time.time - _openTime > 0.5f) // Prevent double press
        {
            if(GetChoosenOption() == null) return; // case not buying anything


            OnOptionChoosen?.Invoke(this);
            Close();
            _isPlaying = false;
            if(PlayerCore.Instance != null)
                PlayerCore.Instance.MoveCameraBack();
            StartCoroutine(DisableCollected(_currentRak));

            _itemThrower.Buy(this);
        }
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
}
