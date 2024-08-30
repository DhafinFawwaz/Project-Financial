using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApotekManager : MonoBehaviour
{
    [SerializeField] PopUp _popUpInsufficient;
    [SerializeField] PopUp _popUpEmptyStock;
    [SerializeField] PopUp _popUpConfirm;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] TextMeshProUGUI _stockEmptyText;
    [SerializeField] WorldUI _worldUI;


    [System.Serializable]
    public class Medicine
    {
        public string Name;
        public int Price = 5000;
        public int Health = 5;
        public int Happiness = 5;

        public int Stock = 10;
    }

    [SerializeField] Medicine[] _medicines;
    int _currentItemIdx;
    public void Buy(int itemIdx)
    {
        _currentItemIdx = itemIdx;
        if(_medicines[itemIdx].Stock <= 0)
        {
            _stockEmptyText.text = $"Maaf stok {_medicines[itemIdx].Name} habis! Silakan kembali dalam {SaveData.DaysUntilNewStock()} hari.";
            _popUpEmptyStock.Show();
            return;
        }
        _priceText.text = _medicines[itemIdx].Price.ToString();
        _messageText.text = $"Yakin ingin membeli {_medicines[itemIdx].Name}?\n(+{_medicines[itemIdx].Health} Health)";
        if(Save.Data.CashMoney < _medicines[itemIdx].Price) _popUpInsufficient.Show();
        else _popUpConfirm.Show();
    }

    public void Confirm()
    {
        Save.Data.CashMoney -= _medicines[_currentItemIdx].Price;
        Save.Data.Health += _medicines[_currentItemIdx].Health;
        Save.Data.Happiness += _medicines[_currentItemIdx].Happiness;
        _medicines[_currentItemIdx].Stock--;
        Save.Data.HealthItemStocks[_currentItemIdx]--;
        _popUpConfirm.Hide();
        _worldUI.RefreshKTP();
        _itemOnHovers[_currentItemIdx].Refresh();
    }  



    [SerializeField] ApotekItemOnHover[] _itemOnHovers;

    void Start()
    {
        for(int i = 0 ; i < _itemOnHovers.Length ; i++)
        {
            _medicines[i].Stock = Save.Data.HealthItemStocks[i];
            _itemOnHovers[i].SetData(_medicines[i]);
        }
    }
}
