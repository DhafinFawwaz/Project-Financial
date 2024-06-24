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
    [SerializeField] int _viewCounter = 1000;
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
    [SerializeField] AnimationUI _closeGateHumanX;
    [SerializeField] AnimationUI _closeGateShadow;

    [Header("Buku Tabungan")]
    [SerializeField] RectTransformAnimation _bukuTabunganRTA;
    [SerializeField] TextMeshProUGUI _tanggalText;
    [SerializeField] TextMeshProUGUI _pemasukanText;
    [SerializeField] TextMeshProUGUI _pengeluaranText;
    [SerializeField] TextMeshProUGUI _pemasukanTotalText;
    [SerializeField] TextMeshProUGUI _pengeluaranTotalText;
    [SerializeField] TextMeshProUGUI _pajakBulananText;
    [SerializeField] TextMeshProUGUI _cicilanKreditText;




    public static FiscalGuardianData CurrentFiscalGuardianData;
    int _currentPeopleIndex = 0;
    public void StartGame()
    {
        _commentSection.Play();
        _views.SetAndAnimate(0, 1000, 0.5f);

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
            _pemasukanText.text += catatan.Pemasukan + "\n";
            _pengeluaranText.text += catatan.Pengeluaran + "\n";
        }
        _pemasukanTotalText.text = people.TotalPemasukan.ToString();
        _pengeluaranTotalText.text = people.TotalPengeluaran.ToString();
        _pajakBulananText.text = people.PajakBulanan.ToString();
        _cicilanKreditText.text = people.CicilanKredit.ToString();
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
            _closeGateHumanCeklisAnimation.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            IncreaseViews();
        }
        else // Shadow, accepted
        {
            _closeGateShadow.Play();
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
            _topText.SetText(CurrentFiscalGuardianData.People[_currentPeopleIndex].WrongMessage)
                .Show()
                .SetOnceComplete(() => {
                    _topText.Hide();
                    DecreaseViews();
                    _closeGateHumanX.Play();
            }).Play();
        }
        else // Shadow, denied
        {
            _closeGateShadow.Play();
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
            _sceneTransition.StartSceneTransition("AfterStreaming");
            return;
        }
        Refresh();
        _peopleAppearAnimation.Play();
    }


    public void IncreaseViews()
    {
        _views.SetAndAnimate(_viewCounter, (_viewCounter += Random.Range(400, 600)), 0.5f);
        _upViews.SetActive(true);
        _downViews.SetActive(false);
        HappyFace();
    }

    public void DecreaseViews()
    {
        int random = Mathf.Max(_viewCounter - Random.Range(400, 600), 0);
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
