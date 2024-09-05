using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FiscalGuardian : StreamingGames
{
    [SerializeField] FiscalGuardianData[] _data;

    [Header("Dialog")]
    [SerializeField] bool _isStartWithDialog = false;
    [SerializeField] RectTransformAnimation _textRTA;
    [SerializeField] TopText _topText;
    [TextArea]
    [SerializeField] string _message;
    [SerializeField] GameObject _dialogCloser;

    [Header("Story")]
    [SerializeField] GameObject _firstTimeDialog;
    [SerializeField] UnityEvent _onFirstTime;
    void Awake()
    {
        if(Save.Data.CurrentDay == 0) {
            CurrentFiscalGuardianData = _data[Save.Data.CurrentDayData.StreamingCounter % _data.Length];
        } else {
            int rand = UnityEngine.Random.Range(0, _data.Length);
            CurrentFiscalGuardianData = _data[rand];
        }
    }

    public override void Play()
    {
        if(Save.Data.CurrentDay == 0 && Save.Data.CurrentDayData.StreamingCounter == 0) LoadDialog();
        else StartCoroutine(DelayAwake());
    }

    IEnumerator DelayAwake()
    {
        yield return new WaitForSeconds(0.1f);
        _dialogCloser.SetActive(false);
        _textRTA.SetEnd(Vector3.one).TweenLocalScale();
        _topText.SetText(_message).Show().Play().SetOnceComplete(() => {
            _dialogCloser.SetActive(true);
            CloseStartDialog();
        });
    }

    public void CloseStartDialog()
    {
        _textRTA.SetEnd(Vector3.zero).TweenLocalScale();
        StartGame();
        _dialogCloser.SetActive(false);
    }


    void LoadDialog()
    {
        _firstTimeDialog.gameObject.SetActive(true);
        _onFirstTime?.Invoke();
    }



    // Game
    [Header("People")]
    [SerializeField] People[] _peoples;
    [SerializeField] RectTransformAnimation _topTextRTA;
    [SerializeField] RectTransformAnimation _ktpRTA;
    [SerializeField] GraphicsAnimation _blackFadeBG;
    [SerializeField] RectTransformAnimation _slideRTA;
    [SerializeField] Image _peopleImg;
    [SerializeField] Image _ktpImg;
    // [SerializeField] TopText _topText; // when people is angry, this will show up


    [Header("Stamp")]
    [SerializeField] Stamp _yesStamp;
    [SerializeField] Stamp _noStamp;

    [Header("Animation")]
    [SerializeField] AnimationUI _peopleAppearAnimation;
    [SerializeField] AnimationUI _closeGateHumanCeklisAnimation;
    [SerializeField] AnimationUI _closeGateHumanXAnimation;
    [SerializeField] AnimationUI _closeGateShadowCeklisAnimation;
    [SerializeField] AnimationUI _closeGateShadowXAnimation;

    [Header("Buku Tabungan")]
    [SerializeField] RectTransformAnimation _bukuTabunganRTA;
    [SerializeField] TextMeshProUGUI _tanggalText;
    [SerializeField] TextMeshProUGUI _pemasukanText;
    [SerializeField] TextMeshProUGUI _pengeluaranText;
    [SerializeField] TextMeshProUGUI _pemasukanTotalText;
    [SerializeField] TextMeshProUGUI _pengeluaranTotalText;
    [SerializeField] TextMeshProUGUI _pajakBulananText;
    [SerializeField] TextMeshProUGUI _cicilanKreditText;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _peopleLeft;

    [Header("GameOver")]
    [SerializeField] TextMeshProUGUI _correntText;
    [SerializeField] TextMeshProUGUI _wrongText;
    [SerializeField] GameObject[] _starGameOvers;
    [SerializeField] AnimationUI _gameEndAnimation;


    int _correctCount = 0;
    int _wrongCount = 0;


    public static FiscalGuardianData CurrentFiscalGuardianData;
    int _currentPeopleIndex = 0;


    public void StartGame()
    {
        _peopleAppearAnimation.Play();
        // Make copy of CurrentFiscalGuardianData
        CurrentFiscalGuardianData = Instantiate(CurrentFiscalGuardianData);
        // Shuffle the people
        for (int i = 0; i < CurrentFiscalGuardianData.People.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, CurrentFiscalGuardianData.People.Length);
            var temp = CurrentFiscalGuardianData.People[i];
            CurrentFiscalGuardianData.People[i] = CurrentFiscalGuardianData.People[randomIndex];
            CurrentFiscalGuardianData.People[randomIndex] = temp;
        }

        Refresh();
    }

    public void Refresh()
    {
        FiscalGuardianPeople people = CurrentFiscalGuardianData.People[_currentPeopleIndex];
        _peopleImg.sprite = _peoples[people.Id].Face;
        _ktpImg.sprite = _peoples[people.Id].KTP;

        // Buku Tabungan
        _tanggalText.text = "";
        _pemasukanText.text = "";
        _pengeluaranText.text = "";
        _pemasukanTotalText.text = "";
        _pengeluaranTotalText.text = "";
        _pajakBulananText.text = "";
        _cicilanKreditText.text = "";

        foreach (var catatan in people.Catatan)
        {
            _tanggalText.text += catatan.Tanggal + "\n";
            _pemasukanText.text += catatan.Pemasukan.ToStringRupiahFormat() + "\n";
            _pengeluaranText.text += catatan.Pengeluaran.ToStringRupiahFormat() + "\n";
        }
        _pemasukanTotalText.text = people.TotalPemasukan.ToStringRupiahFormat();
        _pengeluaranTotalText.text = people.TotalPengeluaran.ToStringRupiahFormat();
        _pajakBulananText.text = people.PajakBulanan.ToStringRupiahFormat();
        _cicilanKreditText.text = people.CicilanKredit.ToStringRupiahFormat();


        _peopleLeft.text = (_currentPeopleIndex+1) + "/" + CurrentFiscalGuardianData.People.Length.ToString();
    }


    void OnEnable()
    {
        _yesStamp.OnStamped += OnAccepted;
        _noStamp.OnStamped += OnDenied;
    }

    void OnDisable()
    {
        _yesStamp.OnStamped -= OnAccepted;
        _noStamp.OnStamped -= OnDenied;
    }

    public void ToggleKTP(bool isOpen)
    {
        if(isOpen) 
        {
            _blackFadeBG.gameObject.SetActive(true);
            _blackFadeBG.SetEndColor(new Color(0, 0, 0, 0.6f)).Play();
            _ktpRTA.SetEnd(Vector3.one).TweenLocalScale();
            _slideRTA.SetEnd(new Vector2(700, -110)).TweenPosition();
        }
        else 
        {
            _blackFadeBG.SetEndColor(new Color(0, 0, 0, 0)).SetOnceEnd(() => {
                _blackFadeBG.gameObject.SetActive(false);
            }).Play();
            _ktpRTA.SetEnd(Vector3.zero).TweenLocalScale();
            _slideRTA.SetEnd(new Vector2(1040, -110)).TweenPosition();
            _yesStamp.GoBack();
            _noStamp.GoBack();
        }
    }


    void OnAccepted()
    {
        StartCoroutine(AcceptedAnimation());
    }   
    void OnDenied()
    {
        StartCoroutine(DeniedAnimation());
    }

    [SerializeField] GameObject _inputBlocker;
    IEnumerator AcceptedAnimation()
    {
        _inputBlocker.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        ToggleKTP(false);

        // Human, accepted
        if(!CurrentFiscalGuardianData.People[_currentPeopleIndex].IsShadow)
        {
            _correctCount++;
            _closeGateHumanCeklisAnimation.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            IncreaseViews();
        }
        else // Shadow, accepted
        {
            _wrongCount++;
            _closeGateShadowCeklisAnimation.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            DecreaseViews();
        }
    }

    IEnumerator DeniedAnimation()
    {
        _inputBlocker.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        ToggleKTP(false);
        
        // Human, denied
        if(!CurrentFiscalGuardianData.People[_currentPeopleIndex].IsShadow)
        {
            _wrongCount++;
            _topText.SetText(CurrentFiscalGuardianData.People[_currentPeopleIndex].WrongMessage)
                .Show()
                .SetOnceComplete(() => {
                    _topText.Hide();
                    DecreaseViews();
                    _closeGateHumanXAnimation.Play();
            }).Play();
        }
        else // Shadow, denied
        {
            _correctCount++;
            _closeGateShadowXAnimation.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            IncreaseViews();
        }

    }

    [SerializeField] SceneTransition _sceneTransition;
    public void LoadNext()
    {
        _currentPeopleIndex++;
        if(_currentPeopleIndex == CurrentFiscalGuardianData.People.Length)
        {
            // End of the game
            HandleGameEnd();
            return;
        }
        Refresh();
        _peopleAppearAnimation.Play();
    }

    void HandleGameEnd()
    {
        long profit = (long) ((10.0f-_wrongCount)/10.0f * (UnityEngine.Random.Range(30000, 40000)));
        profit = (long)Mathf.RoundToInt(profit/1000) * 1000;
        // Save.Data.NeedsMoney += profit;
        // Save.Data.Health -= 10;
        // Save.Data.Happiness -= (_wrongCount*4) - 7;

        Save.Data.CurrentDayData.StreamingCounter++;
        Save.Data.CurrentDayData.GainedViews += _viewCounter;
        Save.Data.CurrentDayData.GainedSubscriber += (long)(_viewCounter * UnityEngine.Random.Range(0.5f, 0.75f));
        Save.Data.CurrentDayData.GainedMoney = (long)(_viewCounter * UnityEngine.Random.Range(100f, 150f));
        Save.Data.Health -= 10;
        Save.Data.Happiness -= 10;
        // Save.Data.CurrentDayData.GainedMoney += profit;


        // AfterStreaming.Penonton += _viewCounter;
        // AfterStreaming.NewSubscriber += _correctCount;
        // AfterStreaming.Penghasilan += profit;
        // AfterStreaming.TotalSubscriber += Save.Data.SubscriberAmount;
        // AfterStreaming.GainedSubscriberEachDay = Save.Data.GainedSubscriberEachDay;

        _correntText.text = (CurrentFiscalGuardianData.People.Length - _wrongCount).ToString();
        _wrongText.text = _wrongCount.ToString();

        if(_wrongCount == 0) _starGameOvers[2].SetActive(true);
        else if(CurrentFiscalGuardianData.People.Length == _wrongCount) _starGameOvers[0].SetActive(true);
        else _starGameOvers[1].SetActive(true);

        Debug.Log(_wrongCount);
        Debug.Log(CurrentFiscalGuardianData.People.Length);

        _gameEndAnimation.Play();
    }



    float viewsIncreaseDelta()
    {
        return _viewCounter * UnityEngine.Random.Range(0.2f, 0.4f);
    } 

    float viewsDecreaseDelta()
    {
        return Mathf.Max(_viewCounter, 500) * UnityEngine.Random.Range(0.07f, 0.15f);
    } 


    public void IncreaseViews()
    {
        _viewCounter += (int)viewsIncreaseDelta();
        OnIncreaseViews?.Invoke(_viewCounter);
    }

    public void DecreaseViews()
    {
        _viewCounter = Mathf.Max(_viewCounter - (int)viewsDecreaseDelta(), 0);
        OnDecreaseViews?.Invoke(_viewCounter);
    }



    public void ToggleBukuTabungan(bool isOpen)
    {
        if(isOpen) 
        {
            _blackFadeBG.gameObject.SetActive(true);
            _blackFadeBG.SetEndColor(new Color(0, 0, 0, 0.6f)).Play();
            _bukuTabunganRTA.SetEnd(Vector3.one).TweenLocalScale();
        }
        else 
        {
            _blackFadeBG.SetEndColor(new Color(0, 0, 0, 0)).SetOnceEnd(() => {
                _blackFadeBG.gameObject.SetActive(false);
            }).Play();
            _bukuTabunganRTA.SetEnd(Vector3.zero).TweenLocalScale();
        }
    }
}
