using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSession : MonoBehaviour
{
    [SerializeField] Option _optionPrefab;
    [SerializeField] Transform _optionContainer;
    [SerializeField] Transform _eToSelesai;
    List<Option> _activeOptions = new List<Option>();
    bool _isPlaying = false;    
    Rak _currentRak;
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
    }
    IEnumerator ScalingAnimation()
    {
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
        StartCoroutine(TweenLocalScaleAnimation(_eToSelesai as RectTransform, _eToSelesai.localScale, Vector3.zero, 0.2f, Ease.OutQuart));
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
            Close();
            _isPlaying = false;
            PlayerCore.Instance.MoveCameraBack();
            StartCoroutine(DisableCollected(_currentRak));
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
