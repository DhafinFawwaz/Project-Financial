using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaterialAnimator : MonoBehaviour
{
    [SerializeField] Sprite[] _spriteList;
    [SerializeField] Material _material;
    [SerializeField] float _animationDuration = 1;
    [SerializeField] bool _loop = false;
    [SerializeField] bool _playOnAwake = false;

    int _currentSpriteIndex = 0;
    float _timeElapsed = 0;
    bool _isPlaying = false;

    void Awake()
    {
        if (_playOnAwake) Play();
    }

    public void SetFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex >= _spriteList.Length) return;
        _currentSpriteIndex = frameIndex;
        _material.mainTexture = _spriteList[_currentSpriteIndex].texture;
    }

    public void SetFirstFrame() => SetFrame(0);
    public void SetLastFrame() => SetFrame(_spriteList.Length - 1);

    public void Play() => StartCoroutine(PlayAnimation());

    IEnumerator PlayAnimation()
    {
        _isPlaying = true;
        while (_isPlaying)
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= _animationDuration)
            {
                _timeElapsed = 0;
                _currentSpriteIndex++;
                if (_currentSpriteIndex >= _spriteList.Length)
                {
                    if (_loop) _currentSpriteIndex = 0;
                    else _isPlaying = false;
                }
                _material.mainTexture = _spriteList[_currentSpriteIndex].texture;
            }
            yield return null;
        }
    }
    


}
