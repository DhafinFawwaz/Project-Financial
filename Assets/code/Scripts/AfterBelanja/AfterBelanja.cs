using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class AfterBelanja : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _barangText;
    [SerializeField] TextMeshProUGUI _hargaText;
    [SerializeField] TextMeshProUGUI _totalhargaText;
    [SerializeField] TextMeshProUGUI _sisaText;
    [SerializeField] TextMeshProUGUI _addedHealthText;
    [SerializeField] TextMeshProUGUI _addedHapinesssisaText;
    [SerializeField] QualityBar _qualityBar;
    [SerializeField] GameObject _kebutuhanDialog;
    [SerializeField] GameObject _kreditDialog;

    [SerializeField] KTPWorld _ktpWorld;


    static int _totalItems = 0;
    static int _totalPercentage = 0;
    public static void SetData(int totalItems, int totalPercentage) {
        _totalItems = totalItems;
        _totalPercentage = totalPercentage;
    }

    [SerializeField] BudgetingData _budgetingData;

    [SerializeField] AnimationUI _kebutuhanAnimation;
    [SerializeField] AnimationUI _kreditAnimation;

    
    void Awake()
    {
        if(Save.Data.CurrentTotalItems == 0) return;
        
        float val = (float)Save.Data.CurrentQuality/Save.Data.CurrentTotalItems/100f;
        int health = (int)(Save.Data.CurrentPredictedHealth * val);
        int happiness = (int)(Save.Data.CurrentPredictedHappiness * val);
        _qualityBar.SetAndAnimate(val, health, happiness, 0, 0);

        // Debug.Log("Quality: " + Save.Data.CurrentQuality);
        // Debug.Log("TotalItems: " + Save.Data.CurrentTotalItems);

        // int health = (int)_budgetingData.PredictHealth(Save.Data.CurrentNeedsMoney, Save.Data.CurrentDay);
        // int happiness = (int)_budgetingData.PredictHappiness(Save.Data.CurrentNeedsMoney, Save.Data.CurrentDay);
        _addedHealthText.text = (health * val).ToString();
        _addedHapinesssisaText.text = (happiness * val).ToString();
        // _qualityBar.SetEndFill((float)val/100).Play();
        // _qualityBar.SetAndAnimate(val, (int)(health*val), (int)(happiness*val));


        _barangText.text = "";
        _hargaText.text = "";

        var _listCart = Save.Data.CurrentListBelanja;
        

        foreach(var item in _listCart)
        {
            _barangText.text += $"{item.Item.Name} x{item.Count}\n";
            _hargaText.text += $"{(item.Item.Price*item.Count).ToStringRupiahFormat()}\n";
        }

        long totalHarga = 0;
        foreach(var item in _listCart)
        {
            totalHarga += item.Item.Price * item.Count;
        }

        _totalhargaText.text = totalHarga.ToStringRupiahFormat();
        _sisaText.text = (Save.Data.NeedsMoney).ToStringRupiahFormat();
        // _addedHealthText.text = _addedHealth.ToString();
        // _addedHapinesssisaText.text = _addedhappiness.ToString();




        // Save.Data.Health += _addedHealth;
        // Save.Data.Happiness += _addedhappiness;
        
        this.Invoke(() => {
            _ktpWorld.SetMoney(Save.Data.NeedsMoney)
                .SetHappiness(Save.Data.Happiness)
                .SetHealth(Save.Data.Health);
        }, 0.1f);


        Save.Data.CurrentQuality = 0;
        Save.Data.CurrentTotalItems = 0;

        if(Save.Data.NeedsMoney < 0) {
            Save.Data.NeedsMoney = Save.Data.TempNeedsMoney;
            Save.Data.CurrentDayData.CreditMoney = totalHarga;
            _sisaText.text = Save.Data.NeedsMoney.ToStringRupiahFormat();
            _kreditDialog.SetActive(true);
            _kebutuhanDialog.SetActive(false);
            _kreditAnimation.Play();
        } else {
            _kreditDialog.SetActive(false);
            _kebutuhanDialog.SetActive(true);
            _kebutuhanAnimation.Play();
        }

        Save.Data.TempNeedsMoney = Save.Data.NeedsMoney;
    }


    [SerializeField] UnityEvent onE;
    void Update()
    {
        if(InputManager.GetKeyDown(KeyCode.E))
        {
            onE?.Invoke();
        }
    }
}
