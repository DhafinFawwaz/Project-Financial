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
    void GenerateCalculation(float[] pieValues)
    {
        long needsMoney = (long)(Save.Data.TotalMoney * pieValues[2]);
        // Debug.Log("needsMoney: " + needsMoney);
        // Debug.Log(Save.Data.NeedsMoney + " - " + Save.Data.DesireMoney + " - " + Save.Data.DebitMoney);
        _kebutuhanText.text = needsMoney.ToStringRupiahFormat();
        _kebutuhanDiv3Text.text = (needsMoney / 3).ToStringRupiahFormat();
        
        int happiness = Clamp0to100toInt(_budgetingData.PredictHappiness(needsMoney, Save.Data.CurrentDay));
        _happinessText.text = happiness.ToString();

        int health = Clamp0to100toInt(_budgetingData.PredictHealth(needsMoney, Save.Data.CurrentDay));
        _healthText.text = health.ToString();

        Save.Data.CurrentPredictedHappiness = happiness;
        Save.Data.CurrentPredictedHealth = health;
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


    void OnPieValuesChanged(float[] values)
    {
        GenerateCalculation(values);
    }

    void Awake()
    {   
        float[] values = new float[3] {1f/3f, 1f/3f, 1f/3f};
        GenerateCalculation(values);
    }
}
