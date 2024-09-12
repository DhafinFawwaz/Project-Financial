using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquiggleMovement : MonoBehaviour
{
    [SerializeField] float _strength = 1;
    void Update()
    {
        // noise movement like camera shake slow
        float x = Mathf.PerlinNoise(Time.time, 0) * 2 - 1;
        float y = Mathf.PerlinNoise(0, Time.time) * 2 - 1;
        transform.localPosition = new Vector3(x, y, 0) * _strength;
        
    }
}
