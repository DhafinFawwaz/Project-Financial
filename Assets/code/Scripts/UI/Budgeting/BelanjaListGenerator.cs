using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BelanjaListGenerator : MonoBehaviour
{
    [SerializeField] float _debounceDuration = 0.1f;
    [SerializeField] float _happinessRatio = 0.2f;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] BelanjaList _belanjaList;
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] ItemData[] _itemData;

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
        // Debounce(GenerateList, _debounceDuration);

        GenerateCalculation();
    }


    [Header("Calculation")]
    [SerializeField] TextMeshProUGUI _kebutuhanText;
    [SerializeField] TextMeshProUGUI _kebutuhanDiv3Text;
    
    [Header("Data")]
    [SerializeField] BudgetingData _budgetingData;
    void GenerateCalculation()
    {
        long needsMoney = (long)(Save.Data.NeedsMoney * _currentValue);
        _kebutuhanText.text = needsMoney.ToStringRupiahFormat();
        _kebutuhanDiv3Text.text = (needsMoney / 3).ToStringRupiahFormat();
        _happinessText.text = Clamp0to100(_budgetingData.PredictHappiness(needsMoney, Save.Data.CurrentDay)).ToString("F2");
        _healthText.text = Clamp0to100(_budgetingData.PredictHealth(needsMoney, Save.Data.CurrentDay)).ToString("F2");
    }
    float Clamp0to100(float value)
    {
        return Mathf.Clamp(value, 0, 100);
    }


    Coroutine _debounceCoroutine;
    IEnumerator DebounceCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
    void Debounce(Action action, float delay)
    {
        if (_debounceCoroutine != null) StopCoroutine(_debounceCoroutine);
        _debounceCoroutine = StartCoroutine(DebounceCoroutine(action, delay));
    }

    [ContextMenu("Generate List")]
    public void GenerateList()
    {
        _belanjaList.Clear();
        Generate();
        Refresh();
    }

    void Refresh()
    {
        _belanjaList.CalculateInfo(out double totalHealthInfo, out double totalHappinessInfo, out long totalPriceInfo);
        if(_healthText)
        _healthText.text = totalHealthInfo.ToString("F2");
        if(_happinessText)
        _happinessText.text = totalHappinessInfo.ToString("F2");
        if(_moneyText)
        _moneyText.text = totalPriceInfo.ToStringRupiahFormat();
    }


    // pick random items 
    // the total price of the items should be less than or equal to needsMoney
    // max of each item is 3
    // calculate the total health and happiness
    void Generate()
    {
        long needsMoney = (long)(Save.Data.NeedsMoney * _currentValue);
        int maxRetries = 20;
        int retries = 0;
        while(retries++ < maxRetries)
        {
            int maxLoop = 200;
            int loop = 0;
            while(_belanjaList.CalculateTotalPrive() < needsMoney && loop++ < maxLoop)
            {
                ItemData item = _itemData[UnityEngine.Random.Range(0, _itemData.Length)];

                _belanjaList.CalculateInfo(out double totalHealth, out double totalHappiness, out long totalPrice);
                
                if(totalHealth + totalHappiness > 0) {
                    double newHappinessRatio = (totalHappiness+item.Happiness) / (totalHealth + totalHappiness);
                    if(newHappinessRatio > _happinessRatio) continue;
                }

                _belanjaList.AddItem(item);
            }
            if(loop >= maxLoop)
            {
                _belanjaList.Clear();
                continue;
            }


            _belanjaList.CalculateInfo(out double totalHealthB, out double totalHappinessB, out long totalPriceB);
            if(totalHealthB + totalHappinessB > 0) {
                double newHappinessRatio = totalHappinessB / (totalHealthB + totalHappinessB);
                if(newHappinessRatio > _happinessRatio) {
                    continue;
                }
            }


            break;
        }
    }


    [ContextMenu("Generate")]
    public void GenerateBelanjaList()
    {
        long needsMoneyDiv3 = Save.Data.CurrentNeedsMoneyDiv3;
    
        int maxLoop = 1000;
        int loop = 0;

        long maxMoney = (long) (needsMoneyDiv3 * (float)_budgetingData.BudgetingDayData[Save.Data.CurrentDay].PercentageFromNeedsMoneyDiv3/100f);

        while(loop++ < maxLoop){
            ItemData item = _itemData[UnityEngine.Random.Range(0, _itemData.Length)];
            if(_belanjaList.CalculateTotalPrive() + item.Price > maxMoney) break;

            _belanjaList.AddItem(item);
        }
        if(_belanjaList.ListToBuy.Count == 0) {
            ItemData item = _itemData[UnityEngine.Random.Range(0, _itemData.Length)];
            _belanjaList.AddItem(item);
            ItemData item2 = _itemData[UnityEngine.Random.Range(0, _itemData.Length)];
            _belanjaList.AddItem(item2);
        }
        else if(_belanjaList.ListToBuy.Count == 1) {
            ItemData item = _itemData[UnityEngine.Random.Range(0, _itemData.Length)];
            _belanjaList.AddItem(item);
        }

        else if(_belanjaList.ListToBuy.Count > 2) {
            _belanjaList.ListToBuy.RemoveAt(_belanjaList.ListToBuy.Count - 1);
        }
        Save.Data.CurrentListBelanja = _belanjaList.ListToBuy;
    }   
    
}
