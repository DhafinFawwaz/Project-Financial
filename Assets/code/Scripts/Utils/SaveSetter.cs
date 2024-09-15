using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSetter : MonoBehaviour
{
    [SerializeField] int _day = 0;

// if we build with including this script, this will give error
    [SerializeField] DayState _dayState = DayState.JustGotHome;
    [SerializeField] bool _isBungaShooterUnlocked = false;
    [SerializeField] bool _disableBGM = false;

    void Awake()
    {
        Save.Data.CurrentDay = _day;
        Save.Data.DayState = _dayState;
        Save.Data.IsBungaShooterBought = _isBungaShooterUnlocked;
        Debug.Log("Day and state set to " + _day + " and " + _dayState);
        if(_disableBGM) {
            Audio.SetMusicMixerVolume(-100);
        }
    }
}
