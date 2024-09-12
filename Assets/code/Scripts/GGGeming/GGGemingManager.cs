using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GGGemingManager : MonoBehaviour
{
    [SerializeField] ItemSeller[] _kursiLevels;
    [SerializeField] ItemSeller[] _monitorLevels;
    [SerializeField] ItemSeller[] _otherLevels;
    [SerializeField] WorldUI _worldUI;

    void Awake()
    {
        Refresh();
    }

    public void UpgradeKursi()
    {
        Save.Data.KursiLevel++;
        Refresh();
    }

    public void UpgradeMonitor()
    {
        Save.Data.MonitorLevel++;
        Refresh();
    }

    public void UpgradeOther()
    {
        Save.Data.OtherLevel++;
        Refresh();
    }

    public void Refresh()
    {
        DisableAll();
        EnableAvailableUpgrade();
        _worldUI.RefreshKTP();
    }

    void DisableAll()
    {
        foreach(var item in _kursiLevels) {
            item.gameObject.SetActive(false);
        }
        foreach(var item in _monitorLevels) {
            item.gameObject.SetActive(false);
        }
        foreach(var item in _otherLevels) {
            item.gameObject.SetActive(false);
        }
    }

    // Enable if not max level
    void EnableAvailableUpgrade()
    {
        if(Save.Data.KursiLevel != _kursiLevels.Length) {
            _kursiLevels[Save.Data.KursiLevel].gameObject.SetActive(true);
        }
        if(Save.Data.MonitorLevel != _monitorLevels.Length) {
            _monitorLevels[Save.Data.MonitorLevel].gameObject.SetActive(true);
        }
        if(Save.Data.OtherLevel != _otherLevels.Length) {
            _otherLevels[Save.Data.OtherLevel].gameObject.SetActive(true);
        }
    }

}
