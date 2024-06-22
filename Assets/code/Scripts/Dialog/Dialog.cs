using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using System;
public class Dialog : MonoBehaviour
{
    [SerializeField] DialogData _data;
    public static Action<DialogData> s_OnDialogFinished;
    [SerializeField] Image _actorImage;
    [SerializeField] TextMeshProUGUI _actorName;
    [SerializeField] TextMeshProUGUI _dialogText;
    [SerializeField] float _delayEachCharacter = 0.05f;
    [SerializeField] bool _playOnStart = false;
    [SerializeField] AnimationUI _animationUI;

    [Header("Dialog boxes")]
    [SerializeField] GameObject _dialogAngkasa;
    [SerializeField] GameObject _dialogBaik;
    [SerializeField] GameObject _dialogSus;

    [Header("Actor Sprites")]
    [SerializeField] Sprite _angkasaSprite;
    [SerializeField] Sprite[] _otherSprites;

    void Start()
    {
        if(_playOnStart) Play();    
    }
    int _currentContentIndex = 0;

    bool _isPlaying = false;
    public void SetDataAndPlay(DialogData data)
    {
        _data = data;
        _currentContentIndex = 0;
        _dialogText.text = "";
        _dialogText.maxVisibleCharacters = 0;
        Play();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if(_isPlaying)
            {
                Next();
            }
        }
    }

    public void Next()
    {
        if(_currentContentIndex < _data.Content.Length)
        {
            _actorName.text = _data.Content[_currentContentIndex].ActorRight;
            _dialogText.text = _data.Content[_currentContentIndex].Text;
            StartCoroutine(TextAnimation());

            if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Angkasa) {
                _dialogAngkasa.SetActive(true);
                _dialogBaik.SetActive(false);
                _dialogSus.SetActive(false);
                _actorName.gameObject.SetActive(false);
            } else if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Baik) {
                _dialogAngkasa.SetActive(false);
                _dialogBaik.SetActive(true);
                _dialogSus.SetActive(false);
                _actorName.gameObject.SetActive(true);
            } else if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Sus) {
                _dialogAngkasa.SetActive(false);
                _dialogBaik.SetActive(false);
                _dialogSus.SetActive(true);
                _actorName.gameObject.SetActive(true);
            }
            _currentContentIndex++;
        } else {
            Stop();
        }
    }

    public void Skip()
    {
        _currentContentIndex = _data.Content.Length;
        _dialogText.maxVisibleCharacters = _dialogText.text.Length;
        Stop();
    }

    public void Play()
    {
        _isPlaying = true;
        _animationUI.Play();
    }   

    public void Stop()
    {
        _isPlaying = false;
        _animationUI.PlayReversed();
        s_OnDialogFinished?.Invoke(_data);
    }

    byte _key = 0;
    IEnumerator TextAnimation()
    {
        byte requirement = ++_key;
        for(int i = 0; i < _dialogText.text.Length; i++)
        {
            _dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(_delayEachCharacter);
            if(_key != requirement) break;
        }
    }
}
