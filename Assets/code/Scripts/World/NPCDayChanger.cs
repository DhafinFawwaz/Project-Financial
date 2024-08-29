using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDayChanger : MonoBehaviour
{
    [SerializeField] int _currentDay = 0;
    void Start()
    {
        if(_currentDay < 0) _currentDay = Save.Data.CurrentDay;

        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(Save.Data.CurrentDay).gameObject.SetActive(true);
        foreach(Transform child in transform) {
            if(!child.gameObject.activeInHierarchy) {
                Destroy(child.gameObject);
            }
        }
    }
}
