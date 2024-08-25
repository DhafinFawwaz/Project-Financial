using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpriteMaterialAnimator : MonoBehaviour
{
    [SerializeField] Sprite[] _spriteList;
    [SerializeField] Material _material;
    [SerializeField] float _animationDuration = 1;
    [SerializeField] bool _loop = false;
    [SerializeField] bool _playOnAwake = false;
    [SerializeField] bool _playOnEnable = false;
    public Action OnAnimationFinished;

    int _currentSpriteIndex = 0;
    float _timeElapsed = 0;
    bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    void Awake()
    {
        if (_playOnAwake) Play();
    }

    void OnEnable()
    {
        if (_playOnEnable) {
            _isPlaying = false;
            Play();
        }
    }

    public void SetFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex >= _spriteList.Length) return;
        _currentSpriteIndex = frameIndex;
        _material.mainTexture = _spriteList[_currentSpriteIndex].texture;
    }

    public void SetFirstFrame() => SetFrame(0);
    public void SetLastFrame() => SetFrame(_spriteList.Length - 1);

    public void Play()
    {
        if(_isPlaying) return;
        SetFirstFrame();
        _timeElapsed = 0;
        StartCoroutine(PlayAnimation());
    }

    public void Stop()
    {
        _isPlaying = false;
    }

    IEnumerator PlayAnimation()
    {
        _isPlaying = true;
        while (_isPlaying)
        {
            _timeElapsed += Time.deltaTime / _animationDuration;
            _currentSpriteIndex = Mathf.FloorToInt(_timeElapsed * _spriteList.Length);
            if (_currentSpriteIndex >= _spriteList.Length)
            {
                if (_loop)
                {
                    _currentSpriteIndex = 0;
                    _timeElapsed = 0;
                }
                else
                {
                    _currentSpriteIndex = _spriteList.Length - 1;
                    break;
                }
            }
            _material.mainTexture = _spriteList[_currentSpriteIndex].texture;
            yield return null;
        }

        if(_isPlaying) OnAnimationFinished?.Invoke();
        _isPlaying = false;
    }
    


}
