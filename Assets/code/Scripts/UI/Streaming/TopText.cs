using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopText : MonoBehaviour
{
    [SerializeField] RectTransformAnimation _animation;
    [SerializeField] TextMeshProUGUI _dialogText;
    [SerializeField] float _delayEachCharacter = 0.01f;
    Action _onceComplete;
    public TopText SetOnceComplete(Action action)
    {
        _onceComplete = action;
        return this;
    } 

    public TopText Show()
    {
        _animation.SetEnd(Vector3.one).TweenLocalScale();
        return this;
    }

    public TopText Hide()
    {
        _animation.SetEnd(Vector3.zero).TweenLocalScale();
        return this;
    }

    public TopText SetText(string text)
    {
        _dialogText.text = text;
        return this;
    }
    public void SetTextImmediete(string text) => SetText(text);

    public TopText Play()
    {
        StartCoroutine(TextAnimation());
        return this;
    }

    public void PlayImmediete() => Play();

    byte _key = 0;
    IEnumerator TextAnimation()
    {
        byte requirement = ++_key;
        for(int i = 0; i < _dialogText.text.Length; i++)
        {
            _dialogText.maxVisibleCharacters = i+1;
            yield return new WaitForSeconds(_delayEachCharacter);
            if(_key != requirement) break;
        }
        _dialogText.maxVisibleCharacters = _dialogText.text.Length;
        yield return new WaitForSeconds(1f);
        _onceComplete?.Invoke();
        _onceComplete = null;
    }
}
