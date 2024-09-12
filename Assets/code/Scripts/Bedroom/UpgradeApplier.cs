using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeApplier : MonoBehaviour
{
    [SerializeField] GameObject[] _kursiLevels;
    [SerializeField] GameObject[] _kursiSitLevels;
    [SerializeField] GameObject[] _monitorLevels;
    [SerializeField] GameObject[] _otherLevels;

    void Awake()
    {
        foreach(var k in _kursiLevels) {
            k.SetActive(false);
        }
        foreach(var k in _kursiSitLevels) {
            k.SetActive(false);
        }
        foreach(var k in _monitorLevels) {
            k.SetActive(false);
        }
        foreach(var k in _otherLevels) {
            k.SetActive(false);
        }

        _kursiLevels[Save.Data.KursiLevel].SetActive(true);
        // _kursiSitLevels[Save.Data.KursiLevel].SetActive(true);
        _monitorLevels[Save.Data.MonitorLevel].SetActive(true);
        _otherLevels[Save.Data.OtherLevel].SetActive(true);
    }

    public void Sit()
    {
        _kursiLevels[Save.Data.KursiLevel].SetActive(false);
        _kursiSitLevels[Save.Data.KursiLevel].SetActive(true);
    }
}
