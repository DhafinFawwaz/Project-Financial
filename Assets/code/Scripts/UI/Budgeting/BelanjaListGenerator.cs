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
        _pieChart.OnPieValuesChanged += OnPieValuesChanged;
    }

    void OnDisable()
    {
        _pieChart.OnPieValuesChanged -= OnPieValuesChanged;
    }


    float _currentValue;
    void OnPieValuesChanged(float[] values)
    {
        if(_currentValue == values[2]) return;
        _currentValue = values[2];
        Debounce(GenerateList, _debounceDuration);
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

    public void GenerateList()
    {
        _belanjaList.Clear();


        long needsMoney = (long)(Save.Data.NeedsMoney * _currentValue);

        
    
        // pick random items 
        // the total price of the items should be less than or equal to needsMoney
        // max of each item is 3
        // calculate the total health and happiness


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





        _belanjaList.CalculateInfo(out double totalHealthInfo, out double totalHappinessInfo, out long totalPriceInfo);
        _healthText.text = totalHealthInfo.ToString("F2");
        _happinessText.text = totalHappinessInfo.ToString("F2");
        _moneyText.text = totalPriceInfo.ToStringRupiahFormat();

    }
    
}
