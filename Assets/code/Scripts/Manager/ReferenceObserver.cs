using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceObserver : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] HUDManager _hudManager;

    void OnEnable()
    {
        PlayerStats.s_OnPlayerMoneyUpdated += _hudManager.OnPlayerMoneyUpdated;
    }
    void OnDisable()
    {
        PlayerStats.s_OnPlayerMoneyUpdated -= _hudManager.OnPlayerMoneyUpdated;
    }
}
