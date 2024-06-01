using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMessenger : MonoBehaviour
{
    [SerializeField] string _tag = "Player";
    [SerializeField] UnityEvent _onTriggerEnter;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(_tag))
        {
            _onTriggerEnter?.Invoke();
        }
    }

    [SerializeField] UnityEvent _onTriggerExit;
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(_tag))
        {
            _onTriggerExit?.Invoke();
        }
    }
}
