using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightUI : MonoBehaviour
{
    [SerializeField] Image _rechargeBar;
    void OnEnable()
    {
        Flashlight.s_OnRecharging += OnRecharging;
    }

    void OnDisable()
    {
        Flashlight.s_OnRecharging -= OnRecharging;
    }

    void OnRecharging(float energy)
    {
        _rechargeBar.fillAmount = energy;
    }
}
