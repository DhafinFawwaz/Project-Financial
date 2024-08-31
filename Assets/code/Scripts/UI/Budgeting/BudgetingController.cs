using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BudgetingController : MonoBehaviour
{
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] TextMeshProUGUI[] _textFields;
    [SerializeField] TextMeshProUGUI _kreditText;
    [SerializeField] TMP_InputField _transferInput;

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
        _kreditText.text = Save.Data.CreditMoney.ToStringCurrencyFormat();
        _transferInput.contentType = TMP_InputField.ContentType.DecimalNumber;
        _transferInput.onSubmit.AddListener((string s) => Transfer());
        _transferInput.onEndEdit.AddListener((string s) => {
            if(long.TryParse(s, out long result)) {
                if(result > Save.Data.CreditMoney) {
                    _transferInput.text = Save.Data.CreditMoney.ToString();
                }
            }
        });
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
        }
    }



    // Kredit transfer
    [SerializeField] TransformAnimation[] _moneyObjs;
    [SerializeField] TransformAnimation[] _moneyObjsSmall;
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
            if(!isCurrent) continue;
            
            _moneyObjs[i].transform.localScale = Vector3.one * 0.5f;
            _moneyObjsSmall[i].transform.localScale = Vector3.one * 0.5f;

            _moneyObjs[i].SetEase(Ease.OutBackCubic);
            _moneyObjsSmall[i].SetEase(Ease.OutBackCubic);

            _moneyObjs[i].TweenLocalScale();
            _moneyObjsSmall[i].TweenLocalScale();
        }
    }

    [SerializeField] PopUp _confirmPopUp;
    [SerializeField] TextMeshProUGUI _confirmText;
    long amount = 0;
    string _type = "";
    public void Transfer()
    {
        amount = long.Parse(_transferInput.text);
        if(amount <= 0) {
            _popUpText.text = "Jumlah uang harus lebih dari 0!";
            _popUp.Show();
            return;
        }

        if(Save.Data.CreditMoney < amount) {
            amount = Save.Data.CreditMoney;
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
    }

    public void Confirm()
    {
        if(_page == 0) Save.Data.NeedsMoney -= amount;
        else if(_page == 1) Save.Data.DesireMoney -= amount;
        else if(_page == 2) Save.Data.DebitMoney -= amount;
        Save.Data.CreditMoney -= amount;
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
}
