using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingRepeatAnimation : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _minScale;
    [SerializeField] Vector3 _maxScale;

    [SerializeField] float _period;

    void Update()
    {
        _target.localScale = Vector3.Lerp(_minScale, _maxScale, Mathf.Sin(Time.time/_period) * 0.5f + 0.5f);
    }
}
