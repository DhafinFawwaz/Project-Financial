using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineScale : MonoBehaviour
{
    [SerializeField] float _minScale = 1;
    [SerializeField] float _maxScale = 1.15f;
    [SerializeField] float _period = 1;

    void Update()
    {
        float scale = Mathf.Lerp(_minScale, _maxScale, Mathf.Sin(Time.time * 2 * Mathf.PI / _period) * 0.5f + 0.5f);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
