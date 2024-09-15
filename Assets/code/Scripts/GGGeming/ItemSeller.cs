using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemSeller : Interactable
{
    [SerializeField] long _price = 800000;
    [SerializeField] TextMeshProUGUI _popUpDescriptionText;
    [SerializeField] TextMeshPro _priceText;

    [SerializeField] PopUp _notEnoughtMoneyPopUp;
    [SerializeField] PopUp _confirmPopUp;
    [SerializeField] ButtonUI _buyButton;

    [TextArea]
    [SerializeField] string _descriptionPopUp = "Ingin membeli game Bunga Shooter ? (Akan muncul game baru saat streaming!)";

    [SerializeField] UnityEvent _onConfirmBuy;

    void Awake()
    {
        _priceText.text = _price.ToStringCurrencyFormat();
    }

    protected override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        Buy();
    }

    public void Buy()
    {
        if(Save.Data.DebitMoney > _price) {
            _popUpDescriptionText.text = _descriptionPopUp;
            _buyButton.OnClick.RemoveAllListeners();
            _buyButton.OnClick.AddListener(ConfirmBuy);
            _confirmPopUp.Show();
            InputManager.SetActiveMouseAndKey(false);
        }
        else {
            _notEnoughtMoneyPopUp.Show();
        }
    }


    public void ConfirmBuy()
    {
        Save.Data.DebitMoney -= _price;
        _onConfirmBuy?.Invoke();
        _confirmPopUp.Hide();
        InputManager.SetActiveMouseAndKey(true);
    }
}
