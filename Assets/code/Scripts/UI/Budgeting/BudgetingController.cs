using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BudgetingController : MonoBehaviour
{
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] TextMeshProUGUI[] _textFields;
    [SerializeField] TextMeshProUGUI[] _textPercentFields;
    [SerializeField] TMP_InputField _transferInput;
    [SerializeField] GameObject _confirmButton;

    void Awake()
    {
        Refresh();
    }



    public void Refresh()
    {
        _pieChart.SetPieValues(new float[]{
            Save.Data.DebitMoney,
            Save.Data.DesireMoney,
            Save.Data.NeedsMoney,
        });
        _textFields[0].text = Save.Data.DebitMoney.ToStringCurrencyFormat();
        _textFields[1].text = Save.Data.DesireMoney.ToStringCurrencyFormat();
        _textFields[2].text = Save.Data.NeedsMoney.ToStringCurrencyFormat();

        long total = Save.Data.TotalMoney;
        _textPercentFields[0].text = (Save.Data.DebitMoney / (float)total * 100).ToString("0.0") + "%";
        _textPercentFields[1].text = (Save.Data.DesireMoney / (float)total * 100).ToString("0.0") + "%";
        _textPercentFields[2].text = (Save.Data.NeedsMoney / (float)total * 100).ToString("0.0") + "%";

        _kreditText.text = CurrentCreditMoney.ToStringCurrencyFormat();
        _transferInput.contentType = TMP_InputField.ContentType.DecimalNumber;
        _transferInput.onSubmit.AddListener((string s) => {
            EventSystem.current.SetSelectedGameObject(null);
            Transfer();
        });
        _transferInput.onEndEdit.AddListener((string s) => {
            if(long.TryParse(s, out long result)) {
                if(result > CurrentCreditMoney) {
                    _transferInput.text = CurrentCreditMoney.ToString();
                }
            }
        });

        // Money types
        _moneyObjsText[0].GetComponent<TextMeshProUGUI>().text = Save.Data.NeedsMoney.ToStringRupiahFormat();
        _moneyObjsText[1].GetComponent<TextMeshProUGUI>().text = Save.Data.DesireMoney.ToStringRupiahFormat();
        _moneyObjsText[2].GetComponent<TextMeshProUGUI>().text = Save.Data.DebitMoney.ToStringRupiahFormat();


        // Kredit
        _kreditDayText.text = "Tagihan Hari Ke-" + (_choosenKreditDay + 1);
        _kreditMinimumText.text = "Minimum: 10000";
        _kreditText.text = CurrentCreditMoney.ToStringCurrencyFormat();
        
        _prevButton.SetActive(IsPrevKreditExist());
        _nextButton.SetActive(IsNextKreditExist());
    }


    public void SetMoney()
    {
        float[] values = _pieChart.GetPieValues();
        long total = Save.Data.TotalMoney;
        Save.Data.DebitMoney = (long)(values[0] * total);
        Save.Data.DesireMoney = (long)(values[1] * total);
        Save.Data.NeedsMoney = (long)(values[2] * total);
    }



    void OnEnable()
    {
        _pieChart.OnPieValuesChanged += OnPieValuesChanged;
    }

    void OnDisable()
    {
        _pieChart.OnPieValuesChanged -= OnPieValuesChanged;
    }

    

    void OnPieValuesChanged(float[] values)
    {
        float _totalMoney = Save.Data.TotalMoney;

        for (int i = 0; i < values.Length; i++)
        {
            _textFields[i].text = (values[i] * _totalMoney).ToStringCurrencyFormat();
            _textPercentFields[i].text = (values[i] * 100).ToString("0.0") + "%";
        }
    }



    // Kredit transfer
    [SerializeField] TransformAnimation[] _moneyObjs;
    [SerializeField] TransformAnimation[] _moneyObjsSmall;
    [SerializeField] TransformAnimation[] _moneyObjsText;
    int _page = 0;
    public void NextPage()
    {
        _page++;
        if (_page >= _moneyObjs.Length)
        {
            _page = 0;
        }
        for (int i = 0; i < _moneyObjs.Length; i++)
        {
            bool isCurrent = i == _page;
            _moneyObjs[i].gameObject.SetActive(isCurrent);
            _moneyObjsSmall[i].gameObject.SetActive(isCurrent);
            _moneyObjsText[i].gameObject.SetActive(isCurrent);
            if(!isCurrent) continue;
            
            _moneyObjs[i].transform.localScale = Vector3.one * 0.5f;
            _moneyObjsSmall[i].transform.localScale = Vector3.one * 0.5f;
            _moneyObjsText[i].transform.localScale = Vector3.one * 0.5f;

            _moneyObjs[i].SetEase(Ease.OutBackCubic);
            _moneyObjsSmall[i].SetEase(Ease.OutBackCubic);
            _moneyObjsText[i].SetEase(Ease.OutBackCubic);

            _moneyObjs[i].TweenLocalScale();
            _moneyObjsSmall[i].TweenLocalScale();
            _moneyObjsText[i].TweenLocalScale();
        }
    }

    [SerializeField] PopUp _confirmPopUp;
    [SerializeField] TextMeshProUGUI _confirmText;
    long amount = 0;
    string _type = "";
    public void Transfer()
    {
        try{
            amount = long.Parse(_transferInput.text);
        } catch {
            return;
        }


        if(amount <= 0) {
            _popUpText.text = "Jumlah uang harus lebih dari 0!";
            _popUp.Show();
            return;
        }

        if(CurrentCreditMoney < amount) {
            amount = CurrentCreditMoney;
            _transferInput.text = amount.ToString();
        }

        if(_page == 0) // kebutuhan
        {
            if(Save.Data.NeedsMoney < amount) {
                NotEnoughtMoney("Uang Kebutuhan");
                return;
            }
            _type = "Uang Kebutuhan";
        }
        else if(_page == 1) // keinginan
        {
            if(Save.Data.DesireMoney < amount) {
                NotEnoughtMoney("Uang Keinginan");
                return;
            }
            _type = "Uang Keinginan";
        }
        else if(_page == 3) // debit
        {
            if(Save.Data.DebitMoney < amount) {
                NotEnoughtMoney("Uang Debit");
                return;
            }
            _type = "Uang Debit";
        }
        _confirmText.text = "Apakah anda yakin ingin mentransfer " + amount.ToStringRupiahFormat() + " dari " + _type + " ke Kredit?";
        _confirmPopUp.Show();
        EventSystem.current.SetSelectedGameObject(_confirmButton);
    }

    public void Confirm()
    {
        if(_page == 0) Save.Data.NeedsMoney -= amount;
        else if(_page == 1) Save.Data.DesireMoney -= amount;
        else if(_page == 2) Save.Data.DebitMoney -= amount;
        CurrentCreditMoney -= amount;
        Refresh();
        _confirmPopUp.Hide();
    }



    [SerializeField] PopUp _popUp;
    [SerializeField] TextMeshProUGUI _popUpText;
    public void NotEnoughtMoney(string type)
    {
        _popUpText.text = type + " tidak cukup! Coba ganti dengan uang lain atau atur ulang alokasi uang!";
        _popUp.Show();
    }



    [Header("Kredit")]
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _prevButton;
    [SerializeField] TextMeshProUGUI _kreditText;
    [SerializeField] TextMeshProUGUI _kreditDayText;
    [SerializeField] TextMeshProUGUI _kreditMinimumText;
    int _choosenKreditDay = 0;
    long CurrentCreditMoney {
        get => Save.Data.DayDatas[_choosenKreditDay].CreditMoney;
        set => Save.Data.DayDatas[_choosenKreditDay].CreditMoney = value;
    }
    bool IsNextKreditExist()
    {
        for (int i = _choosenKreditDay + 1; i < Save.Data.DayDatas.Count; i++)
        {
            if(Save.Data.DayDatas[i].CreditMoney > 0) return true;
        }
        return false;
    }
    bool IsPrevKreditExist()
    {
        for (int i = _choosenKreditDay - 1; i >= 0; i--)
        {
            if(Save.Data.DayDatas[i].CreditMoney > 0) return true;
        }
        return false;
    }
    public void NextKreditPage()
    {
        _choosenKreditDay++;
        while(CurrentCreditMoney == 0 && _choosenKreditDay < Save.Data.DayDatas.Count - 1) {
            _choosenKreditDay++;
        }
        Refresh();
    }
    public void PrevKreditPage()
    {
        _choosenKreditDay--;
        while(CurrentCreditMoney == 0 && _choosenKreditDay > 0) {
            _choosenKreditDay--;
        }
        Refresh();
    }
}
