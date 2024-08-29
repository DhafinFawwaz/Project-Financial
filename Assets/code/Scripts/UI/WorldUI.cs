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

    [Header("For Apotek")]    
    [SerializeField] bool _ktpOnly = false;


    void Awake()
    {
        RefreshStates();
    }

    public void RefreshStates()
    {
        _belanjaList.SetList(Save.Data.CurrentListBelanja);
        
        _dateText.text = (Save.Data.CurrentDay+1).ToString();

        if(_isBelanja) HandleBelanjaState();
        else HandleWorldState();

        RefreshKTP();

        if(_ktpOnly)
        {
            _bottomLeftPhoneBelanja.gameObject.SetActive(false);
            _bottomLeftPhoneWorld.gameObject.SetActive(false);
            _leftHandParent.gameObject.SetActive(false);
            _tasBottomRightParent.gameObject.SetActive(false);
            _tasRightParent.gameObject.SetActive(false);
        }
    }

    void HandleBelanjaState()
    {
        _bottomLeftPhoneBelanja.gameObject.SetActive(true);
        _bottomLeftPhoneWorld.gameObject.SetActive(false);
        _leftHandParent.gameObject.SetActive(false);
        _tasBottomRightParent.gameObject.SetActive(false);
        _tasRightParent.gameObject.SetActive(true);


        _ktpBelanja.gameObject.SetActive(true);
        _ktpWorld.gameObject.SetActive(false);


        _addedHappiness = 0;
        _addedHealth = 0;
        _currentBelanjaMoney = Save.Data.CurrentBelanjaMoney;
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

    }

    public void RefreshKTP()
    {
        _ktpBelanja.SetMoneyTop(Save.Data.DesireMoney)
            .SetMoneyBottom(Save.Data.CurrentBelanjaMoney)
            .SetGreenBarFill(1)
            .SetHapiness(Save.Data.Happiness, 0)
            .SetHealth(Save.Data.Health, 0);

        _ktpWorld.SetMoney(Save.Data.CashMoney)
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
    static double _addedHappiness = 0;
    static double _addedHealth = 0;
    static double _currentBelanjaMoney = 0;
    public static double AddedHappiness { get => _addedHappiness; set => _addedHappiness = value; }
    public static double AddedHealth { get => _addedHealth; set => _addedHealth = value; }
    public static double CurrentBelanjaMoney { get => _currentBelanjaMoney; set => _currentBelanjaMoney = value; }
    public void AddItemFromOption(OptionSession option)
    {
        ItemData itemData = Instantiate(option.OptionData.ItemData);
        Option choosenOption = option.GetChoosenOption();
        for(int i = 0; i < choosenOption.BuyCount; i++)
        {
            _belanjaList.AddToCart(itemData);        
        }

        _currentBelanjaMoney -= itemData.Price * choosenOption.BuyCount;
        _addedHappiness += itemData.Happiness * choosenOption.BuyCount;
        _addedHealth += itemData.Health * choosenOption.BuyCount;

        _ktpBelanja.SetMoneyTop(Save.Data.CashMoney)
            .SetMoneyBottom((long)_currentBelanjaMoney)
            .SetGreenBarFill((float)_currentBelanjaMoney / (float)Save.Data.CurrentBelanjaMoney)
            .SetHapiness(Save.Data.Happiness, _addedHappiness)
            .SetHealth(Save.Data.Health, _addedHealth);

    }


    [SerializeField] SceneTransition _sceneTransition;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // _sceneTransition.StartSceneTransition("MainMenu");
        }
    }
}
