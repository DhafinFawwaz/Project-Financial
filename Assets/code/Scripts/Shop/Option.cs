using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Option : MonoBehaviour
{
    bool _diskon;
    bool _inflasi;
    bool _buy1get1;
    int _buyCount;
    int _price;
    int _health;
    int _happiness;
    [SerializeField] GameObject _diskonGO;
    [SerializeField] GameObject _inflasiGO;
    [SerializeField] GameObject _buy1get1GO;
    [SerializeField] TextMeshPro _buyCountText;
    [SerializeField] TextMeshPro _priceText;
    [SerializeField] TextMeshPro _healthText;
    [SerializeField] TextMeshPro _happinessText;

    public void SetValues(
        bool diskon,
        bool inflasi,
        bool buy1get1,
        int buyCount,
        int price,
        int health,
        int happiness
    )
    {
        _diskon = diskon;
        _inflasi = inflasi;
        _buy1get1 = buy1get1;
        _buyCount = buyCount;
        _price = price;
        _health = health;
        _happiness = happiness;
    }
    public void IncrementBuyCount()
    {
        _buyCount++;
        _buyCountText.text = "x"+_buyCount.ToString();
    }
    public void ResetBuyCount()
    {
        _buyCount = 0;
        _buyCountText.text = "x"+_buyCount.ToString();
    }

    public void Refresh()
    {
        _diskonGO.SetActive(_diskon);
        _inflasiGO.SetActive(_inflasi);
        _buy1get1GO.SetActive(_buy1get1);
        _buyCountText.text = "x"+_buyCount.ToString();
        _priceText.text = _price.ToString();
        _healthText.text = _health.ToString();
        _happinessText.text = _happiness.ToString();
    }
}
