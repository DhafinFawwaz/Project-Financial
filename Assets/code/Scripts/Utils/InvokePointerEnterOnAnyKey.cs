using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InvokePointerEnterOnAnyKey : MonoBehaviour
{
    [SerializeField] EventTrigger eventTrigger;
    void Reset()
    {
        eventTrigger = GetComponent<EventTrigger>();
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            Debug.Log("what");
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(eventTrigger.gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
        }
    }
}
