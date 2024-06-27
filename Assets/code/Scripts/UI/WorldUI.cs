using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    [SerializeField] BelanjaList _belanjaList;
    public BelanjaList BelanjaList => _belanjaList;
    [SerializeField] KTPBelanja _ktpBelanja;
    [SerializeField] KTPWorld _ktpWorld;
    [SerializeField] bool _isBelanja = false;
    [SerializeField] GameObject _bottomLeftPhoneBelanja;
    [SerializeField] GameObject _bottomLeftPhoneWorld;
    [SerializeField] GameObject _leftHandParent;
    [SerializeField] GameObject _tasBottomRightParent;
    [SerializeField] GameObject _tasRightParent;
    [SerializeField] TextMeshProUGUI _dateText;
    [SerializeField] Timer _diskonTimer;
    [SerializeField] Timer _inflasiTimer;

    float _addedHappiness;
    float _addedHealth;
    public float AddedHappiness { get => _addedHappiness; set => _addedHappiness = value; }
    public float AddedHealth { get => _addedHealth; set => _addedHealth = value; }

    void Awake()
    {
        _belanjaList.SetList(Save.Data.CurrentListBelanja);
        if(_isBelanja) HandleBelanjaState();
        else HandleWorldState();

        StartGame();
    }

    void HandleBelanjaState()
    {
        _bottomLeftPhoneBelanja.gameObject.SetActive(true);
        _bottomLeftPhoneWorld.gameObject.SetActive(false);
        _leftHandParent.gameObject.SetActive(false);
        _tasBottomRightParent.gameObject.SetActive(false);
        _tasRightParent.gameObject.SetActive(true);

        _dateText.text = Save.Data.CurrentDay.ToString();

        _ktpBelanja.gameObject.SetActive(true);
        _ktpWorld.gameObject.SetActive(false);
        _ktpBelanja.SetMoneyTop(Save.Data.Money)
            .SetMoneyBottom(Save.Data.CurrentBelanjaMoney)
            .SetGreenBarFill(1)
            .SetHapiness(Save.Data.Happiness, 0)
            .SetHealth(Save.Data.Health, 0);
    }

    void HandleWorldState()
    {
        _bottomLeftPhoneBelanja.gameObject.SetActive(false);
        _bottomLeftPhoneWorld.gameObject.SetActive(true);
        _leftHandParent.gameObject.SetActive(true);
        _tasBottomRightParent.gameObject.SetActive(true);
        _tasRightParent.gameObject.SetActive(false);

        _ktpBelanja.gameObject.SetActive(false);
        _ktpWorld.gameObject.SetActive(true);
        _ktpWorld.SetMoney(Save.Data.CurrentBelanjaMoney)
            .SetHappiness(Save.Data.Happiness)
            .SetHealth(Save.Data.Health)
            .SetSkillPoint(Save.Data.SkillPoin);
    }


    public void StartGame()
    {
        _inflasiTimer.Begin();
        _diskonTimer.SetTime(10);
        _diskonTimer.Begin();
    }



    // Option
    public void AddItemFromOption(OptionSession option)
    {
        ItemData itemData = Instantiate(option.OptionData.ItemData);
        Option choosenOption = option.GetChoosenOption();
        for(int i = 0; i < choosenOption.BuyCount; i++)
        {
            _belanjaList.AddToCart(itemData);        
        }
    }
}
