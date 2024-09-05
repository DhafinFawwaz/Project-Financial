using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "BudgetingData", menuName = "BudgetingData")]
public class BudgetingData : ScriptableObject
{
    [SerializeField] BudgetingDayData[] _budgetingDayData;
    public BudgetingDayData[] BudgetingDayData => _budgetingDayData;

    public float PredictHappiness(long needsMoney, int day) {
        int idx = day;
        if(idx >= _budgetingDayData.Length) idx = _budgetingDayData.Length - 1;
        return Remap(needsMoney, _budgetingDayData[idx].MinNeedsMoney, _budgetingDayData[idx].MaxNeedsMoney, _budgetingDayData[idx].MinHappiness, _budgetingDayData[idx].MaxHappiness);   
    }

    public float PredictHealth(long needsMoney, int day) {
        int idx = day;
        if(idx >= _budgetingDayData.Length) idx = _budgetingDayData.Length - 1;
        return Remap(needsMoney, _budgetingDayData[idx].MinNeedsMoney, _budgetingDayData[idx].MaxNeedsMoney, _budgetingDayData[idx].MinHealth, _budgetingDayData[idx].MaxHealth);   
    }

    float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

[System.Serializable]
public class BudgetingDayData
{
    public long MinNeedsMoney;
    public long MaxNeedsMoney;
    public long MinHealth;
    public long MaxHealth;
    public long MinHappiness;
    public long MaxHappiness;
    public long PercentageFromNeedsMoneyDiv3 = 100;
}
