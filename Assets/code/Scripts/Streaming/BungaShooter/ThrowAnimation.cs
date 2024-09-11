using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnimation : MonoBehaviour
{
    [SerializeField] float _height;
    [SerializeField] float _minLength;
    [SerializeField] float _maxLength;
    [SerializeField] float _lifeTime = 1;
    [SerializeField] float _duration;
    [SerializeField] bool _destroyOnDone = true;
    [SerializeField] float _rotatePeriod = 0.5f;
    public void Throw()
    {
        StartCoroutine(PlayThrow());
    }

    IEnumerator PlayThrow()
    {
        float initTime = Time.time;
        float throwLength = Random.Range(_minLength, _maxLength);
        float negative = Random.Range(0, 2) == 0 ? 1 : -1;
        throwLength *= negative;


        Vector3 initPos = transform.position;
        float initialZrotation = transform.rotation.eulerAngles.z;
        while(Time.time - initTime < _lifeTime)
        {
            float t = (Time.time - initTime) / _duration;
            transform.position = new Vector3(initPos.x + throwLength * t, initPos.y + _height * Parabole(t), initPos.z);
            transform.rotation = Quaternion.Euler(0, 0, initialZrotation + 360 * t / _rotatePeriod * -negative);
            yield return null;
        }

        if(_destroyOnDone)
        {
            Destroy(gameObject);
        }
    }

    float Parabole(float x) => -4 * x * x + 4 * x;
}
