using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSeller : Interactable
{
    [SerializeField] long _price = 800000;
    [SerializeField] TextMeshPro _priceText;
    [SerializeField] TextMeshProUGUI _popUpDescriptionText;

    [SerializeField] PopUp _notEnoughtMoneyPopUp;
    [SerializeField] PopUp _confirmPopUp;
    [SerializeField] PopUp _alreadyBoughtPopUp;
    const string DESCRIPTION_POPUP = "Ingin membeli game Bunga Shooter ? (Akan muncul game baru saat streaming!)";

    [SerializeField] Renderer[] _toDisableWhenBought;

    void Awake()
    {
        _priceText.text = _price.ToStringRupiahFormat();
        if(Save.Data.IsBungaShooterBought) {
            foreach(var item in _toDisableWhenBought) {
                item.enabled = false;
            }
        }
    }

    public void BuyBungaShooter()
    {
        if(Save.Data.IsBungaShooterBought) {
            _alreadyBoughtPopUp.Show();
            return;
        }

        if(Save.Data.DesireMoney > _price) {
            _popUpDescriptionText.text = DESCRIPTION_POPUP;
            _confirmPopUp.Show();
        }
        else {
            _notEnoughtMoneyPopUp.Show();
        }
    }

    public void ConfirmBuyBungaShooter()
    {
        Save.Data.DesireMoney -= _price;
        Save.Data.IsBungaShooterBought = true;
        _confirmPopUp.Hide();
        // _interactablePromptAnchor.gameObject.SetActive(false);
        StartCoroutine(TweenLocalScale(_interactablePromptAnchor.transform, _interactablePromptAnchor.transform.localScale, Vector3.zero, 0.15f, Ease.InCubic, () => {
            _interactablePromptAnchor.gameObject.SetActive(false);
            foreach(var item in _toDisableWhenBought) {
                item.enabled = false;
            }
        }));
    }
}
