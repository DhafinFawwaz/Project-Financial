using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApotekManager : MonoBehaviour
{
    [SerializeField] PopUp _popUpInsufficient;
    [SerializeField] PopUp _popUpConfirm;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] WorldUI _worldUI;


    [System.Serializable]
    public class Medicine
    {
        public string Name;
        public int Price;
        public int Health;
        public int Happiness;
    }

    [SerializeField] Medicine[] _medicines;
    int _currentItemIdx;
    public void Buy(int itemIdx)
    {
        _currentItemIdx = itemIdx;
        _priceText.text = _medicines[itemIdx].Price.ToString();
        _messageText.text = $"Yakin ingin membeli {_medicines[itemIdx].Name}? (+{_medicines[itemIdx].Health} Health)";
        if(Save.Data.CashMoney < _medicines[itemIdx].Price) _popUpInsufficient.Show();
        else _popUpConfirm.Show();
    }

    public void Confirm()
    {
        Save.Data.CashMoney -= _medicines[_currentItemIdx].Price;
        Save.Data.Health += _medicines[_currentItemIdx].Health;
        Save.Data.Happiness += _medicines[_currentItemIdx].Happiness;
        _popUpConfirm.Hide();
        _worldUI.RefreshKTP();
    }  



    [SerializeField] ApotekItemOnHover[] _itemOnHovers;

    void Start()
    {
        for(int i = 0 ; i < _itemOnHovers.Length ; i++)
        {
            _itemOnHovers[i].SetData(_medicines[i]);
        }
    }
}
