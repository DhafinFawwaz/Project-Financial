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
    [SerializeField] ImageFillAnimation _qualityBar;

    [SerializeField] KTPWorld _ktpWorld;


    static int _totalItems = 0;
    static int _totalPercentage = 0;
    public static void SetData(int totalItems, int totalPercentage) {
        _totalItems = totalItems;
        _totalPercentage = totalPercentage;
    }

    [SerializeField] BudgetingData _budgetingData;

    
    void Awake()
    {
        if(_totalItems == 0) // this means we're debugging
        {
            _totalPercentage = 500;
            _totalItems = 6;
        }
        float val = _totalPercentage/_totalItems;
        _addedHealthText.text = (_budgetingData.PredictHealth(Save.Data.NeedsMoney, Save.Data.CurrentDay) * val).ToString();
        _addedHapinesssisaText.text = (_budgetingData.PredictHappiness(Save.Data.NeedsMoney, Save.Data.CurrentDay) * val).ToString();
        _qualityBar.SetEndFill((float)val/100).Play();


        _barangText.text = "";
        _hargaText.text = "";

        var _listCart = Save.Data.CurrentListBelanja;
        

        foreach(var item in _listCart)
        {
            _barangText.text += $"{item.Item.Name} x{item.Count}\n";
            _hargaText.text += $"{item.Item.Price*item.Count}\n";
        }

        float totalHarga = 0;
        foreach(var item in _listCart)
        {
            totalHarga += item.Item.Price * item.Count;
        }

        _totalhargaText.text = totalHarga.ToString();
        _sisaText.text = (Save.Data.NeedsMoney - totalHarga).ToString();
        // _addedHealthText.text = _addedHealth.ToString();
        // _addedHapinesssisaText.text = _addedhappiness.ToString();




        Save.Data.NeedsMoney -= (long)totalHarga;
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
            // use credit
            // restore
            Save.Data.NeedsMoney = Save.Data.TempNeedsMoney;
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
