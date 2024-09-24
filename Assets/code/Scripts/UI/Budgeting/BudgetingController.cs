using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BudgetingController : MonoBehaviour
{
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] TextMeshProUGUI[] _textFields;
    [SerializeField] TextMeshProUGUI[] _textPercentFields;
    [SerializeField] TMP_InputField _transferInput;
    [SerializeField] GameObject _confirmButton;
    [SerializeField] GameObject _okButton;
    [SerializeField] GameObject _lunasPage;

    [SerializeField] GameObject _clickableCreditButton;
    [SerializeField] GameObject _unclickableCreditButton;

    [Header("Deadline")] 
    [SerializeField] TextMeshProUGUI _deadlineText;
    [SerializeField] GameObject _kreditBackButton;
    [SerializeField] GameObject _kreditBackButtonToPopUp;

    void Awake()
    {
        // Kredit
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


        // Debit
        _transferDebitInput.contentType = TMP_InputField.ContentType.DecimalNumber;
        _transferDebitInput.onSubmit.AddListener((string s) => {
            EventSystem.current.SetSelectedGameObject(null);
            TransferDebit();
        });

        Refresh();
    }



    public void Refresh()
    {
        _pieChart.SetPieValues(new float[]{
            Save.Data.DebitMoney,
            Save.Data.DesireMoney,
            Save.Data.NeedsMoney,
        });
        _textFields[0].text = "<color=#00000066>" + Save.Data.DebitTabunganMoney.ToStringRupiahFormat() + " + <color=#000>" + Save.Data.DebitMoney.ToStringRupiahFormat();
        _textFields[1].text = Save.Data.DesireMoney.ToStringRupiahFormat();
        _textFields[2].text = Save.Data.NeedsMoney.ToStringRupiahFormat();

        long total = Save.Data.TotalMoney;
        _textPercentFields[0].text = (Save.Data.DebitMoney / (float)total * 100).ToString("0.0") + "%";
        _textPercentFields[1].text = (Save.Data.DesireMoney / (float)total * 100).ToString("0.0") + "%";
        _textPercentFields[2].text = (Save.Data.NeedsMoney / (float)total * 100).ToString("0.0") + "%";

        _kreditText.text = CurrentCreditMoney.ToStringRupiahFormat();


        // Kredit

        // Money types
        _moneyObjsText[0].GetComponent<TextMeshProUGUI>().text = "<color=#000>" + Save.Data.DebitTabunganMoney.ToStringRupiahFormat() + " + <color=#00000066>" + Save.Data.DebitMoney.ToStringRupiahFormat();
        _moneyObjsText[1].GetComponent<TextMeshProUGUI>().text = Save.Data.DesireMoney.ToStringRupiahFormat();
        _moneyObjsText[2].GetComponent<TextMeshProUGUI>().text = Save.Data.NeedsMoney.ToStringRupiahFormat();

        // Tagihan
        _kreditDayText.text = "Tagihan Hari Ke-" + (_choosenKreditDay + 1);
        _kreditDeadlineText.text = (_choosenKreditDay + SaveData.KREDIT_DEADLINE).ToString();
        _kreditMinimumText.text = "Minimum: 10000";
        _kreditText.text = CurrentCreditMoney.ToStringRupiahFormat();
        
        _prevButton.SetActive(IsPrevKreditExist());
        _nextButton.SetActive(IsNextKreditExist());

        // if there is no kredit, show lunas
        if(!IsKreditExist()) {
            // show lunas
            _lunasPage.SetActive(true);
            _clickableCreditButton.SetActive(false);
            _unclickableCreditButton.SetActive(true);
        } else {
            _lunasPage.SetActive(false);
            _clickableCreditButton.SetActive(true);
            _unclickableCreditButton.SetActive(false);
            // if the current page credit is 0, find any credit
            if(CurrentCreditMoney == 0) {
                _choosenKreditDay = 0;
                NextKreditPage();
            }
        }



        // Debit

        // Money types
        _moneyDebitObjsText[0].GetComponent<TextMeshProUGUI>().text = Save.Data.DesireMoney.ToStringRupiahFormat();
        _moneyDebitObjsText[1].GetComponent<TextMeshProUGUI>().text = Save.Data.NeedsMoney.ToStringRupiahFormat();

        _debitText.text = Save.Data.DebitTabunganMoney.ToStringRupiahFormat() + "<color=#00000066> + " + Save.Data.DebitMoney.ToStringRupiahFormat();
    


        if((Save.Data.CurrentDay+1) % 3 == 0) {
            _kreditBackButton.SetActive(true);
            _kreditBackButtonToPopUp.SetActive(false);
            if(HasCreditDeadlineToday()) {
                _kreditBackButton.SetActive(false);
                _kreditBackButtonToPopUp.SetActive(true);
            } 
        } else {
            _kreditBackButton.SetActive(false);
            _kreditBackButtonToPopUp.SetActive(false);
            if(HasCreditDeadlineToday()) {
                _kreditBackButton.SetActive(false);
                _kreditBackButtonToPopUp.SetActive(false);
            } 
        }
        // Deadline
        


        // Lose if all money is 0 and credit in deadline not paid
        // if(_choosenKreditDay+3 <= Save.Data.CurrentDay) {
        //     _deadlineText.gameObject.SetActive(true);
        // } else {
        //     _deadlineText.gameObject.SetActive(false);
        // }
        _deadlineText.gameObject.SetActive(true);
        _deadlineText.text = ((_choosenKreditDay + 3) - Save.Data.CurrentDay).ToString() + "hari lagi";
        if(_choosenKreditDay+3 == Save.Data.CurrentDay) {
            _deadlineText.text = "HARI INI!";
        }

        if(IsLoseCredit()) {
            _onLose?.Invoke();
        }
    }

    bool HasCreditDeadlineToday()
    {
        for(int i = 0; i < Save.Data.DayDatas.Count; i++)
        {
            // if <=, we should have already lose.
            if(i+3 == Save.Data.CurrentDay && Save.Data.DayDatas[i].CreditMoney > 0) {
                return true;
            }
        }
        return false;
    }

    bool IsLoseCredit()
    {
        for(int i = 0; i < Save.Data.DayDatas.Count; i++)
        {
            if(i+3 <= Save.Data.CurrentDay) {
                return Save.Data.DebitTabunganMoney == 0
                    && Save.Data.DebitMoney == 0
                    && Save.Data.DesireMoney == 0
                    && Save.Data.NeedsMoney == 0;
            }
        }
        return false;
    }
    [SerializeField] UnityEvent _onLose;


    public void SetMoney()
    {
        float[] values = _pieChart.GetPieValues();
        long total = Save.Data.TotalMoney;
        Save.Data.DebitMoney = (long)(values[0] * total);
        Save.Data.DesireMoney = (long)(values[1] * total);
        Save.Data.NeedsMoney = (long)(values[2] * total);
        Save.Data.CurrentNeedsMoney = Save.Data.NeedsMoney;

        
        // Debug.Log(
        //     "DebitMoney: " + Save.Data.DebitMoney + "\n" +
        //     "DesireMoney: " + Save.Data.DesireMoney + "\n" +
        //     "NeedsMoney: " + Save.Data.NeedsMoney
        // );
        Refresh();
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
            _textFields[i].text = (values[i] * _totalMoney).ToStringRupiahFormat();
            _textPercentFields[i].text = (values[i] * 100).ToString("0.0") + "%";
        }

        _textFields[0].text = "<color=#00000066>" + Save.Data.DebitTabunganMoney.ToStringRupiahFormat() + " + <color=#000>" + (values[0] * _totalMoney).ToStringRupiahFormat();
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
            // _moneyObjsText[i].gameObject.SetActive(isCurrent);
            if(_moneyObjsText[i].transform.parent) _moneyObjsText[i].transform.parent.gameObject.SetActive(isCurrent);
            if(!isCurrent) continue;
            
            _moneyObjs[i].transform.localScale = Vector3.one * 0.5f;
            _moneyObjsSmall[i].transform.localScale = Vector3.one * 0.5f;
            // _moneyObjsText[i].transform.localScale = Vector3.one * 0.5f;
            if(_moneyObjsText[i].transform.parent) _moneyObjsText[i].transform.parent.localScale = Vector3.one * 0.5f;

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
            EventSystem.current.SetSelectedGameObject(_okButton);
            return;
        }

        if(CurrentCreditMoney < amount) {
            amount = CurrentCreditMoney;
            _transferInput.text = amount.ToString();
        }

        if(_page == 0) // kebutuhan
        {
            if(Save.Data.DebitTabunganMoney < amount) {
                NotEnoughtMoney("Uang Debit");
                return;
            }
            _type = "Uang Debit";
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
            if(Save.Data.NeedsMoney < amount) {
                NotEnoughtMoney("Uang Kebutuhan");
                return;
            }
            _type = "Uang Kebutuhan";
        }
        _confirmText.text = "Apakah anda yakin ingin mentransfer " + amount.ToStringRupiahFormat() + " dari " + _type + " ke Kredit?";
        _confirmPopUp.Show();
        EventSystem.current.SetSelectedGameObject(_confirmButton);
    }

    public void Confirm()
    {
        if(_page == 0) {
            Save.Data.DebitTabunganMoney -= amount;
            // Only if its not salary day
            if(Save.Data.DebitTabunganMoney <= 0 && (Save.Data.CurrentDay+1) % 3 != 0) { 
                Save.Data.DebitTabunganMoney = Save.Data.DebitMoney;
                Save.Data.DebitMoney = 0;
            }
        }
        else if(_page == 1) Save.Data.DesireMoney -= amount;
        else if(_page == 2) Save.Data.NeedsMoney -= amount;
        CurrentCreditMoney -= amount;
        Refresh();
        _confirmPopUp.Hide();
        EventSystem.current.SetSelectedGameObject(null);
    }



    [SerializeField] PopUp _popUp;
    [SerializeField] TextMeshProUGUI _popUpText;
    public void NotEnoughtMoney(string type)
    {
        _popUpText.text = type + " tidak cukup! Coba ganti dengan uang lain atau atur ulang alokasi uang!";
        _popUp.Show();
        EventSystem.current.SetSelectedGameObject(_okButton);
    }



    [Header("Kredit")]
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _prevButton;
    [SerializeField] TextMeshProUGUI _kreditText;
    [SerializeField] TextMeshProUGUI _kreditDayText;
    [SerializeField] TextMeshProUGUI _kreditMinimumText;
    [SerializeField] TextMeshProUGUI _kreditDeadlineText;
    int _choosenKreditDay = 0;
    long CurrentCreditMoney {
        get => Save.Data.DayDatas[_choosenKreditDay].CreditMoney;
        set => Save.Data.DayDatas[_choosenKreditDay].CreditMoney = value;
    }

    bool IsKreditExist()
    {
        for (int i = 0; i < Save.Data.DayDatas.Count; i++)
        {
            if(Save.Data.DayDatas[i].CreditMoney > 0) return true;
        }
        return false;
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



    // Debit transfer
    [Header("Debit")]
    [SerializeField] TextMeshProUGUI _debitText;
    [SerializeField] Image _leftToRightImg;
    [SerializeField] TransformAnimation _leftToRightAnim;
    [SerializeField] Sprite _leftToRightSprite;
    [SerializeField] Sprite _rightToLeftSprite;

    [SerializeField] TMP_InputField _transferDebitInput;
    [SerializeField] PopUp _confirmDebitPopUp;
    [SerializeField] GameObject _confirmDebitButton;
    [SerializeField] TextMeshProUGUI _confirmDebitText;
    [SerializeField] TransformAnimation[] _moneyDebitObjs;
    [SerializeField] TransformAnimation[] _moneyDebitObjsSmall;
    [SerializeField] TransformAnimation[] _moneyDebitObjsText;
    
    int _pageDebit = 0;
    bool _leftToRight = true;

    public void ToggleLeftToRight()
    {
        _leftToRight = !_leftToRight;
        _leftToRightImg.sprite = _leftToRight ? _leftToRightSprite : _rightToLeftSprite;
        _leftToRightAnim.transform.localScale = Vector3.one * 1.5f;
        _leftToRightAnim.TweenLocalScale();
    }

    public void NextPageDebit()
    {
        _pageDebit++;
        if (_pageDebit >= _moneyDebitObjs.Length)
        {
            _pageDebit = 0;
        }
        for (int i = 0; i < _moneyDebitObjs.Length; i++)
        {
            bool isCurrent = i == _pageDebit;
            _moneyDebitObjs[i].gameObject.SetActive(isCurrent);
            _moneyDebitObjsSmall[i].gameObject.SetActive(isCurrent);
            _moneyDebitObjsText[i].gameObject.SetActive(isCurrent);
            if(_moneyDebitObjsText[i].transform.parent) _moneyDebitObjsText[i].transform.parent.gameObject.SetActive(isCurrent);
            if(!isCurrent) continue;
            
            _moneyDebitObjs[i].transform.localScale = Vector3.one * 0.5f;
            _moneyDebitObjsSmall[i].transform.localScale = Vector3.one * 0.5f;
            // _moneyDebitObjsText[i].transform.localScale = Vector3.one * 0.5f;
            if(_moneyDebitObjsText[i].transform.parent) _moneyDebitObjsText[i].transform.parent.localScale = Vector3.one * 0.5f;

            _moneyDebitObjs[i].SetEase(Ease.OutBackCubic);
            _moneyDebitObjsSmall[i].SetEase(Ease.OutBackCubic);
            _moneyDebitObjsText[i].SetEase(Ease.OutBackCubic);

            _moneyDebitObjs[i].TweenLocalScale();
            _moneyDebitObjsSmall[i].TweenLocalScale();
            _moneyDebitObjsText[i].TweenLocalScale();
        }
    }

    long amountDebit = 0;   
    public void TransferDebit()
    {
        try{
            amountDebit = long.Parse(_transferDebitInput.text);
        } catch {
            return;
        }

        if(amountDebit <= 0) {
            _popUpText.text = "Jumlah uang harus lebih dari 0!";
            _popUp.Show();
            EventSystem.current.SetSelectedGameObject(_okButton);
            return;
        }

        if(!_leftToRight) {
            if(_pageDebit == 0) // keinginan
            {
                if(Save.Data.DesireMoney < amountDebit) {
                    NotEnoughtMoney("Uang Keinginan");
                    return;
                }
                _type = "Uang Keinginan";
            }
            else if(_pageDebit == 1) // kebutuhan
            {
                if(Save.Data.NeedsMoney < amountDebit) {
                    NotEnoughtMoney("Uang Kebutuhan");
                    return;
                }
                _type = "Uang Kebutuhan";
            }
            else if(_pageDebit == 2) // debit
            {
                if(Save.Data.DebitTabunganMoney < amountDebit) {
                    NotEnoughtMoney("Uang Debit");
                    return;
                }
                _type = "Uang Debit";
            }

            _confirmDebitText.text = "Apakah anda yakin ingin mentransfer " + amountDebit.ToStringRupiahFormat() + " dari " + _type + " ke Debit?";
            _confirmDebitPopUp.Show();
            EventSystem.current.SetSelectedGameObject(_confirmDebitButton);
        
        } else {

            if(Save.Data.DebitTabunganMoney < amountDebit) {
                NotEnoughtMoney("Uang Debit Tabungan");
                return;
            }

            if(_pageDebit == 0) // keinginan
            {
                _type = "Uang Keinginan";
            }
            else if(_pageDebit == 1) // kebutuhan
            {
                _type = "Uang Kebutuhan";
            }

            _confirmDebitText.text = "Apakah anda yakin ingin mentransfer " + amountDebit.ToStringRupiahFormat() + " dari Debit ke " + _type + " ?";
            _confirmDebitPopUp.Show();
            EventSystem.current.SetSelectedGameObject(_confirmDebitButton);
        }

    }
    
    public void ConfirmDebit()
    {
        _confirmDebitPopUp.Hide();
        if(!_leftToRight) { // wallet to debit
            if(_pageDebit == 0) Save.Data.DesireMoney -= amountDebit;
            else if(_pageDebit == 1) Save.Data.NeedsMoney -= amountDebit;
            Save.Data.DebitTabunganMoney += amountDebit;
        } else {
            if(_pageDebit == 0) Save.Data.DesireMoney += amountDebit;
            else if(_pageDebit == 1) Save.Data.NeedsMoney += amountDebit;
            Save.Data.DebitTabunganMoney -= amountDebit;
        }

        Refresh();
        EventSystem.current.SetSelectedGameObject(null);

    }




    public void EnsureChartSafe()
    {
        if(
            Save.Data.DebitMoney == 0
            || Save.Data.DesireMoney == 0
            || Save.Data.NeedsMoney == 0
        ){
            _popUpText.text = "Karena ada uang yang 0, alokasi uang akan diatur otomatis agar chart tidak sulit ditarik.";
            _popUp.Show();

            long total = Save.Data.TotalMoney;
            Save.Data.DebitMoney = total/3;
            Save.Data.DesireMoney = total/3;
            Save.Data.NeedsMoney = total/3;
            Refresh();


            EventSystem.current.SetSelectedGameObject(_okButton);
            return;
        }
    }



    [SerializeField] SceneTransitionStarter _sceneTransitionStarter;
    [SerializeField] BelanjaList _belanjaList;
    [SerializeField] BelanjaListGenerator _belanjaListGenerator;

    [SerializeField] PopUp _kreditDeadlineTodayPopUp;
    public void Exit()
    {
        if(HasCreditDeadlineToday()) {
            _kreditDeadlineTodayPopUp.Show();
            return;
        }

        // if(_belanjaList.ListToBuy.Count <= 0) {
        //     _popUpText.text = "Tolong atur agar belanjaan tidak kosong!";
        //     _popUp.Show();
        //     EventSystem.current.SetSelectedGameObject(_okButton);
        //     return;
        // }
        SetMoney();
        // _belanjaListGenerator.GenerateBelanjaList();
        _sceneTransitionStarter.StartTransition("World");

    }



    public void ResetPieChart()
    {
        var values = new float[]{1f/3, 1f/3, 1f/3};
        _pieChart.SetPieValues(values);
        OnPieValuesChanged(values);
    }
}
