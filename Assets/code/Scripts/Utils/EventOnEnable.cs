using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    [SerializeField] UnityEvent _onEnable;
    void OnEnable()
    {
        _onEnable.Invoke();
    }
}
