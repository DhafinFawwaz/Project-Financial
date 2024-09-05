using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishAnimation : MonoBehaviour
{
    [SerializeField] float _period = 1;
    [SerializeField] float _minScaleX = 1f;
    [SerializeField] float _maxScaleX = 1.15f;
    [SerializeField] float _minScaleY = 1f;
    [SerializeField] float _maxScaleY = 1.15f;

    void Update()
    {
        float scaleX = Mathf.Lerp(_minScaleX, _maxScaleX, Mathf.Sin(Time.time * 2 * Mathf.PI / _period) * 0.5f + 0.5f);
        float scaleY = Mathf.Lerp(_minScaleY, _maxScaleY, Mathf.Sin((Time.time + _period / 2) * 2 * Mathf.PI / _period) * 0.5f + 0.5f);
        

        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}    
