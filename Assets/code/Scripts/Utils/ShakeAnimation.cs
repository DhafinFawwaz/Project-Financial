using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _shakeAmplitude = 100;
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeFrequency = 50f;
    Vector3 initPos;

    void Awake()
    {
        initPos = _target.position;
    }

    public void Play()
    {
        StartCoroutine(Shake());
    }

    byte _shakeKey;
    IEnumerator Shake()
    {
        byte requirement = ++_shakeKey;
        float t = 0;
        while(t <= 1 && requirement == _shakeKey)
        {
            t += Time.deltaTime / _shakeDuration;
            float x = t * _shakeFrequency;
            float y = (1-t) * _shakeAmplitude * Mathf.Sin(x*_shakeFrequency);
            _target.position = initPos + new Vector3(y, 0, 0);
            yield return null;
        }
        if(requirement == _shakeKey) _target.position = initPos;
    }
}
