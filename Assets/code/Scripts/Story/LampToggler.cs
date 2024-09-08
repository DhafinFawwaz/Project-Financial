using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampToggler : MonoBehaviour
{
    [SerializeField] GameObject[] _lights;
    void Start()
    {
        if(Save.Data.DayState == DayState.AfterBelanja){
            foreach(var light in _lights){
                light.SetActive(true);
            }
        } else {
            foreach(var light in _lights){
                light.SetActive(false);
            }
        }
    }
}
