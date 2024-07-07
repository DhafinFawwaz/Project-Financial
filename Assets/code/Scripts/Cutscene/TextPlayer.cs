using System.Collections;
using TMPro;
using UnityEngine;

public class TextPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _dialogText;
    [SerializeField] float _delayEachCharacter = 0.01f;

    [SerializeField] RectTransformAnimation _animation;
    public void Show()
    {
        _animation.SetEnd(Vector3.one).TweenLocalScale();
    }

    public void Hide()
    {
        _animation.SetEnd(Vector3.zero).TweenLocalScale();
    }

    public void SetText(string text)
    {
        _dialogText.text = text;
    }

    public void SetTextAndPlay(string text)
    {
        SetText(text);
        Play();
    }

    public void Play()
    {
        StartCoroutine(TextAnimation());
    }

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
        if(requirement == _key)
            _dialogText.maxVisibleCharacters = _dialogText.text.Length;
    }

}
