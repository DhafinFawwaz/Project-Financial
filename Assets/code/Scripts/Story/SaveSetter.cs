using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSetter : MonoBehaviour
{
    [SerializeField] int _day = 0;

#if UNITY_EDITOR
// if we build with including this script, this will give error
    [SerializeField] DayState _dayState = DayState.JustGotHome;
#endif

    void Awake()
    {
        Save.Data.CurrentDay = _day;
        Save.Data.DayState = _dayState;
        Debug.Log("Day and state set to " + _day + " and " + _dayState);
    }
}
