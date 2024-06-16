using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BudgetingManager : MonoBehaviour
{
    [SerializeField] PieChart _pieChart;
    [SerializeField] BudgetCalculator _budgetCalculator;
    [SerializeField] ListBelanja _listBelanja;  

    void Start()
    {
        _budgetCalculator.SetAndAnimate(10000, 3500);
        _pieChart.SetAndAnimatePie(0.2f, 0.3f, 0.5f);
    }

}
