using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinePosition : MonoBehaviour
{
    [SerializeField] float _period = 1;
    
    [Header("False to use y")]
    [SerializeField]
    bool _useX = true;
    [SerializeField] float _amplitude = 100;

    Vector3 _initialPosition;

    [SerializeField] bool _useLocalPosition = false;

    void Start()
    {
        _initialPosition = transform.position;
        if(_useLocalPosition) _initialPosition = transform.localPosition;
    }

    void Update()
    {
        float t = Time.time / _period;
        float sine = Mathf.Sin(t * Mathf.PI * 2);
        Vector3 position = _initialPosition;
        if (_useX)
        {
            position.x += sine * _amplitude;
        }
        else
        {
            position.y += sine * _amplitude;
        }
        transform.position = position;
        if(_useLocalPosition) transform.localPosition = position;
    }
}
