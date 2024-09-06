using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSeller : MonoBehaviour
{    
    [SerializeField] PopUp _popUpInsufficient;
    [SerializeField] PopUp _popUpEmptyStock;
    [SerializeField] PopUp _popUpConfirm;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] TextMeshProUGUI _stockEmptyText;
    [SerializeField] WorldUI _worldUI;

    void OnEnable()
    {
        Seller.s_OnNPCInteract += OnSellerTriggered;
    }

    void OnDisable()
    {
        Seller.s_OnNPCInteract -= OnSellerTriggered;
    }

    Seller.Snack _snack;
    Seller _seller;
    void OnSellerTriggered(Seller seller)
    {
        _seller = seller;
        _snack = seller.SoldSnack;

        if(_snack.Stock <= 0)
        {
            _stockEmptyText.text = $"Maaf stok {_snack.Name} habis! Silakan coba kembali besok.";
            _popUpEmptyStock.Show();
            return;
        }
        _priceText.text = _snack.Price.ToString();
        _messageText.text = $"Yakin ingin membeli {_snack.Name}?\n(+{_snack.Happiness} Happiness)";
        if(Save.Data.NeedsMoney < _snack.Price) _popUpInsufficient.Show();
        else _popUpConfirm.Show();
    }

    public void Confirm()
    {
        Save.Data.NeedsMoney -= _snack.Price;
        Save.Data.Happiness += _snack.Happiness;
        _snack.Stock--;
        Save.Data.HapinessItemStocks[_snack.Index]--;
        _popUpConfirm.Hide();
        _worldUI.RefreshKTP();
        _seller.Refresh();
        _seller.ThrowItem();
    }  

}
