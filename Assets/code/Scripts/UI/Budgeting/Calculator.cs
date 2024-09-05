using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI _kebutuhanText;
    [SerializeField] TextMeshProUGUI _kebutuhanDiv3Text;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;

    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] BudgetingData _budgetingData;
    void GenerateCalculation()
    {
        long needsMoney = (long)(Save.Data.TotalMoney * _currentValue);
        _kebutuhanText.text = needsMoney.ToStringRupiahFormat();
        _kebutuhanDiv3Text.text = (needsMoney / 3).ToStringRupiahFormat();
        _happinessText.text = Clamp0to100toInt(_budgetingData.PredictHappiness(needsMoney, Save.Data.CurrentDay)).ToString();
        _healthText.text = Clamp0to100toInt(_budgetingData.PredictHealth(needsMoney, Save.Data.CurrentDay)).ToString();
    }
    int Clamp0to100toInt(float value)
    {
        return (int)Mathf.Clamp(value, 0, 100);
    }


    void OnEnable()
    {
        if(_pieChart)
        _pieChart.OnPieValuesChanged += OnPieValuesChanged;
    }

    void OnDisable()
    {
        if(_pieChart)
        _pieChart.OnPieValuesChanged -= OnPieValuesChanged;
    }


    float _currentValue = 1;
    void OnPieValuesChanged(float[] values)
    {
        if(_currentValue == values[2]) return;
        _currentValue = values[2];
        GenerateCalculation();
    }

    void Awake()
    {   
        GenerateCalculation();
    }
}
