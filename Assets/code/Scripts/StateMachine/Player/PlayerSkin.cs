using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerSkin : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] Material _playerMaterial;
    [SerializeField] Sprite _defaultSprite;
    [SerializeField] float _flipVelocityThreshold = 0.02f;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorDepanStart;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorDepan;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorDepanEnd;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorBelakangStart;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorBelakang;
    [SerializeField] SpriteMaterialAnimator _spriteMaterialAnimatorBelakangEnd;

    bool _anyAnimationPlaying => _spriteMaterialAnimatorDepan.IsPlaying || _spriteMaterialAnimatorBelakang.IsPlaying || _spriteMaterialAnimatorDepanStart.IsPlaying || _spriteMaterialAnimatorDepanEnd.IsPlaying || _spriteMaterialAnimatorBelakangStart.IsPlaying || _spriteMaterialAnimatorBelakangEnd.IsPlaying;
    bool _anyDepanPlaying => _spriteMaterialAnimatorDepan.IsPlaying || _spriteMaterialAnimatorDepanStart.IsPlaying || _spriteMaterialAnimatorDepanEnd.IsPlaying;
    bool _anyBelakangPlaying => _spriteMaterialAnimatorBelakang.IsPlaying || _spriteMaterialAnimatorBelakangStart.IsPlaying || _spriteMaterialAnimatorBelakangEnd.IsPlaying;

    bool _isVelocityStopping => _rb.velocity.x < _flipVelocityThreshold && _rb.velocity.x > -_flipVelocityThreshold && _zVelocity < _flipVelocityThreshold && _zVelocity > -_flipVelocityThreshold;

    [SerializeField] bool _swapZtoY = false;
    float _zVelocity { get {
        if(_swapZtoY) return _rb.velocity.y;
        return _rb.velocity.z;
    }}

    enum AnimationState
    {
        Idle,
        Run,
    }
    AnimationState _animationState = AnimationState.Idle;

    void Start()
    {
        _spriteMaterialAnimatorDepanStart.OnAnimationFinished += _spriteMaterialAnimatorDepan.Play;
        _spriteMaterialAnimatorBelakangStart.OnAnimationFinished += _spriteMaterialAnimatorBelakang.Play;

        _playerMaterial.mainTexture = _defaultSprite.texture;
    }

    void Update()
    {
        if (_rb.velocity.x > _flipVelocityThreshold)
        {
            _playerMaterial.mainTextureScale = new Vector2(1, 1);
        }
        else if (_rb.velocity.x < -_flipVelocityThreshold)
        {
            _playerMaterial.mainTextureScale = new Vector2(-1, 1);
        }

        if(_animationState == AnimationState.Idle)
        {
            if (_zVelocity > _flipVelocityThreshold)
            {
                _spriteMaterialAnimatorBelakang.Stop();
                _spriteMaterialAnimatorDepanStart.Play();
                _animationState = AnimationState.Run;
            }
            else if (_zVelocity < -_flipVelocityThreshold)
            {
                _spriteMaterialAnimatorDepan.Stop();
                _spriteMaterialAnimatorBelakangStart.Play();
                _animationState = AnimationState.Run;
            }

            if(_rb.velocity.x > _flipVelocityThreshold || _rb.velocity.x < -_flipVelocityThreshold)
            {
                if(_anyBelakangPlaying)
                {
                    _spriteMaterialAnimatorBelakang.Stop();
                    _spriteMaterialAnimatorBelakangStart.Stop();
                    _spriteMaterialAnimatorBelakangEnd.Stop();
                    _spriteMaterialAnimatorBelakang.Play();
                }
                else if(_anyDepanPlaying)
                {
                    _spriteMaterialAnimatorDepan.Stop();
                    _spriteMaterialAnimatorDepanStart.Stop();
                    _spriteMaterialAnimatorDepanEnd.Stop();
                    _spriteMaterialAnimatorDepan.Play();
                }
                else
                {
                    _spriteMaterialAnimatorBelakang.Play();
                }
                _animationState = AnimationState.Run;
            }
        }

        else if(_animationState == AnimationState.Run)
        {
            if(_zVelocity > _flipVelocityThreshold && 
                (_spriteMaterialAnimatorBelakang.IsPlaying || _spriteMaterialAnimatorBelakangStart.IsPlaying || _spriteMaterialAnimatorBelakangEnd.IsPlaying)
            )
            {
                _spriteMaterialAnimatorBelakang.Stop();
                _spriteMaterialAnimatorBelakangStart.Stop();
                _spriteMaterialAnimatorBelakangEnd.Stop();
                _spriteMaterialAnimatorDepan.Play();
            }
            else if(_zVelocity < -_flipVelocityThreshold && 
                (_spriteMaterialAnimatorDepan.IsPlaying || _spriteMaterialAnimatorDepanStart.IsPlaying || _spriteMaterialAnimatorDepanEnd.IsPlaying)
            )
            {
                _spriteMaterialAnimatorDepan.Stop();
                _spriteMaterialAnimatorDepanStart.Stop();
                _spriteMaterialAnimatorDepanEnd.Stop();
                _spriteMaterialAnimatorBelakang.Play();
            }
        }

        if(_isVelocityStopping)
        {
            
            if(_spriteMaterialAnimatorBelakang.IsPlaying || _spriteMaterialAnimatorBelakangStart.IsPlaying)
            {
                _spriteMaterialAnimatorBelakangEnd.Play();
            }
            else if(_spriteMaterialAnimatorDepan.IsPlaying || _spriteMaterialAnimatorDepanStart.IsPlaying)
            {
                _spriteMaterialAnimatorDepanEnd.Play();
            }
        
            _spriteMaterialAnimatorDepanStart.Stop();
            _spriteMaterialAnimatorBelakangStart.Stop();
            _spriteMaterialAnimatorDepan.Stop();
            _spriteMaterialAnimatorBelakang.Stop();
            _animationState = AnimationState.Idle;
        }


    }

}
