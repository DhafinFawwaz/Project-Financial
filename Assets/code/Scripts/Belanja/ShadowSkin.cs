using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSkin : MonoBehaviour
{
    Vector3 _velocity;
    Vector3 _initLocalScale;
    Vector3 _lastPos;
    [SerializeField] float _flipXThreshold = 0.1f;

    [SerializeField] Transform _skin;

    void Awake()
    {
        _initLocalScale = _skin.localScale;
    }


    int _facing = 1;
    float _lastFlipTime = 0;
    [SerializeField] float _flipCooldown = 0.8f;
    void RefreshFacing()
    {
        if(Time.time - _lastFlipTime < _flipCooldown) return;
        if (_velocity.x > _flipXThreshold && _facing == -1)
        {
            _lastFlipTime = Time.time;
            _facing = 1;
            _skin.localScale = new Vector3(-_initLocalScale.x, _initLocalScale.y, _initLocalScale.z);
        }
        else if (_velocity.x < -_flipXThreshold && _facing == 1)
        {
            _lastFlipTime = Time.time;
            _facing = -1;
            _skin.localScale = new Vector3(_initLocalScale.x, _initLocalScale.y, _initLocalScale.z);
        }
    }


    void Update()
    {
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
        RefreshFacing();
    }
}
