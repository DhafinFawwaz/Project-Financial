using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreamingManager : MonoBehaviour
{
    [SerializeField] GameObject _inputBlocker;
    [SerializeField] CommentSection _commentSection;

    [Header("Face")]
    [SerializeField] TextAnimation _views;
    [SerializeField] int _viewCounter = 500;
    [SerializeField] GameObject _upViews;
    [SerializeField] GameObject _downViews;
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Image _face;
    [SerializeField] GameChoose _gameChoose;
    [SerializeField] CanvasGroup[] _gameCG;
    [SerializeField] GameObject _home;

    [Header("People")]
    [SerializeField] People[] _peoples;
    [SerializeField] RectTransformAnimation _topTextRTA;
    [SerializeField] RectTransformAnimation _ktpRTA;
    [SerializeField] GraphicsAnimation _blackFadeBG;
    [SerializeField] RectTransformAnimation _slideRTA;
    [SerializeField] Image _peopleImg;
    [SerializeField] Image _ktpImg;
    [SerializeField] TopText _topText; // when people is angry, this will show up


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

    void Awake()
    {
        _views.SetAndAnimate(0, Mathf.CeilToInt(Save.Data.SubscriberAmount/100), 0.5f);
    }
    public void StartGame()
    {
        _commentSection.Play();

        _peopleAppearAnimation.Play();
        StartCoroutine(UpdatingFace());

        // Make copy of CurrentFiscalGuardianData
        CurrentFiscalGuardianData = Instantiate(CurrentFiscalGuardianData);
        // Shuffle the people
        for (int i = 0; i < CurrentFiscalGuardianData.People.Length; i++)
        {
            int randomIndex = Random.Range(i, CurrentFiscalGuardianData.People.Length);
            var temp = CurrentFiscalGuardianData.People[i];
            CurrentFiscalGuardianData.People[i] = CurrentFiscalGuardianData.People[randomIndex];
            CurrentFiscalGuardianData.People[randomIndex] = temp;
        }

        Refresh();
    }

    public void InitViewsAnimated()
    {
        _views.SetAndAnimate(0, Mathf.CeilToInt(Save.Data.SubscriberAmount/100), 0.5f);
    }

    IEnumerator UpdatingFace()
    {
        int lastValue = 0;
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 1.4f));
            if(_justChangedFaceForcefully)
            {
                _justChangedFaceForcefully = false;
                continue;
            }

            int val = lastValue;
            while(val == lastValue)
            {
                val = Random.Range(0, _faceSprites.Length-2);
            }
            lastValue = val;
            _face.sprite = _faceSprites[val];
        }
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
        _gameChoose.OnGameSelected += OnGameSelected;
        _yesStamp.OnStamped += OnAccepted;
        _noStamp.OnStamped += OnDenied;
    }

    void OnDisable()
    {
        _gameChoose.OnGameSelected -= OnGameSelected;
        _yesStamp.OnStamped -= OnAccepted;
        _noStamp.OnStamped -= OnDenied;
    }


    public void OnGameSelected(int page)
    {
        if(_gameCG.Length <= page) return;
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_gameCG[page], _gameCG[page].alpha, 1, 0.3f, Ease.OutQuad));
        Invoke(nameof(DisableHome), 0.3f);
    }

    void DisableHome()
    {
        _home.SetActive(false);
    }


    byte _key2 = 0;
    IEnumerator TweenCanvasGroupAlphaAnimation(CanvasGroup cg, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key2;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key2 == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            cg.alpha = Mathf.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key2 == requirement)
        {
            cg.alpha = end;
        }
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
        long profit = (long) ((10.0f-_wrongCount)/10.0f * (Random.Range(30000, 40000)));
        profit = (long)Mathf.RoundToInt(profit/1000) * 1000;
        Save.Data.NeedsMoney += profit;
        Save.Data.Health -= 10;
        Save.Data.Happiness -= (_wrongCount*4) - 7;
        Save.Data.SubscriberAmount += _viewCounter;
        Save.Data.CurrentDayData.StreamingCounter++;

        AfterStreaming.Penonton = _viewCounter;
        AfterStreaming.NewSubscriber = _correctCount;
        AfterStreaming.Penghasilan = profit;
        AfterStreaming.TotalSubscriber = Save.Data.SubscriberAmount;
        Debug.Log("Save.Data.CurrentDayData.GainedSubscriber: " + Save.Data.CurrentDayData.GainedSubscriber);
        Save.Data.CurrentDayData.GainedSubscriber += _viewCounter;
        Debug.Log("Save.Data.CurrentDayData.GainedSubscriber: " + Save.Data.CurrentDayData.GainedSubscriber);
        AfterStreaming.GainedSubscriberEachDay = Save.Data.GainedSubscriberEachDay;

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
        return _viewCounter * Random.Range(0.07f, 0.13f);
    } 

    float viewsDecreaseDelta()
    {
        return _viewCounter * Random.Range(0.03f, 0.07f);
    } 
    public void IncreaseViews()
    {
        int _oldViewCounter = _viewCounter;
        _views.SetAndAnimate(_oldViewCounter, (_viewCounter += (int)viewsIncreaseDelta()), 0.5f);
        _upViews.SetActive(true);
        _downViews.SetActive(false);
        HappyFace();
    }

    public void DecreaseViews()
    {
        int random = Mathf.Max(_viewCounter - (int)viewsDecreaseDelta(), 0);
        _views.SetAndAnimate(_viewCounter, random, 0.5f);
        _viewCounter = random;
        _upViews.SetActive(false);
        _downViews.SetActive(true);
        SadFace();
    }

    bool _justChangedFaceForcefully = false;
    void SadFace()
    {
        _face.sprite = _faceSprites[5];
        _justChangedFaceForcefully = true;
    }
    void HappyFace()
    {
        _face.sprite = _faceSprites[4];
        _justChangedFaceForcefully = true;
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
