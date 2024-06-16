using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BudgetCalculator : MonoBehaviour
{
    [SerializeField] TextAnimation _currentMoney;
    [SerializeField] TextAnimation _totalBought;
    [SerializeField] TextAnimation _totalLeft;


    public void SetAndAnimate(float currentMoney, float totalBought)
    {
        _currentMoney.SetAndAnimate(0, currentMoney, 1f);
        _totalBought.SetAndAnimate(0, totalBought, 1f);
        _totalLeft.SetAndAnimate(0, currentMoney - totalBought, 2f);
    }

    
}
