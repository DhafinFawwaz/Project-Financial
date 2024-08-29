using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] Light[] _light;
    void Start()
    {
        if(Save.Data.DayState == DayState.AfterKuliah) _light[1].gameObject.SetActive(true);
        else if(Save.Data.DayState == DayState.AfterBelanja) _light[2].gameObject.SetActive(true);
        else _light[0].gameObject.SetActive(true);
    }
}
