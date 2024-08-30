using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BudgetCalculator : MonoBehaviour
{
    [SerializeField] TextAnimation _currentMoney;
    [SerializeField] TextAnimation _totalBought;
    [SerializeField] TextAnimation _totalLeft;
    [SerializeField] TextAnimation _health;
    [SerializeField] TextAnimation _happiness;

    void Awake()
    {
        _health.SetPrefix("+");
        _happiness.SetPrefix("+");
    }


    long _currentMoneyValue = 0;
    long _totalBoughtValue = 0;
    long _totalLeftValue = 0;
    double _healthValue = 0;
    double _happinessValue = 0;
    public void SetAndAnimate(long currentMoney, long totalBought)
    {
        _currentMoney.SetAndAnimate(_currentMoneyValue, currentMoney, 0.3f);
        _totalBought.SetAndAnimate(_totalBoughtValue, totalBought, 0.3f);
        _totalLeft.SetAndAnimate(_totalLeftValue, currentMoney - totalBought, 0.3f);
        _currentMoneyValue = currentMoney;
        _totalBoughtValue = totalBought;
        _totalLeftValue = currentMoney - totalBought;
    }

    public void SetAndAnimate(double health, double happiness)
    {
        health = Mathf.Clamp((float)health, 0, 100);
        happiness = Mathf.Clamp((float)happiness, 0, 100);
        _health.SetAndAnimate((float)_healthValue, (float)health, 0.3f);
        _happiness.SetAndAnimate((float)_happinessValue, (float)happiness, 0.3f);
        _healthValue = health;
        _happinessValue = happiness;
    }

    public void SetAndAnimate(List<ItemCount> itemCounts)
    {
        long currentMoney = Save.Data.NeedsMoney;
        long totalBought = 0;
        double totalHealth = 0;
        double totalHappiness = 0;
        foreach (var item in itemCounts)
        {
            totalBought += item.Item.Price * item.Count;
            totalHealth += item.Item.Health * item.Count;
            totalHappiness += item.Item.Happiness * item.Count;
        }
        SetAndAnimate(currentMoney, totalBought);
        SetAndAnimate(totalHealth, totalHappiness);
    }
}
