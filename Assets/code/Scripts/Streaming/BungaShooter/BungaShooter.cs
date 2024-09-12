using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BungaShooter : StreamingGames
{
    [SerializeField] Bunga[] _bungas;
    List<bool> _isBungaShot = new List<bool>();
    [SerializeField] BungaShooterData[] _bungaShooterData;
    BungaShooterData _chosenBungaShooterData;
    [SerializeField] int _minIncrement = 5;
    [SerializeField] int _maxIncrement = 10;
    [Header("UI")]
    [SerializeField] ImageFillAnimation _currentMoneyBarFillAnimation;
    [SerializeField] TextMeshProUGUI _currentMoneyText; 
    [SerializeField] ImageFillAnimation _shotCountBarFillAnimation;
    [SerializeField] TextMeshProUGUI _shotCountText; 

    [Header("Finish")]
    [SerializeField] ImageFillAnimation _currentMoneyBarFillAnimation_Finish;
    [SerializeField] TextMeshProUGUI _currentMoneyText_Finish; 
    [SerializeField] ImageFillAnimation _shotCountBarFillAnimation_Finish;
    [SerializeField] TextMeshProUGUI _shotCountText_Finish; 

    [Header("Sprite")]
    [SerializeField] Sprite _losePopUpSprite;
    [SerializeField] Sprite _winPopUpSprite;
    [SerializeField] Image _popUpImg;

    // Logic
    long _maxMoney = 10000000;
    long _currentMoney = 0;
    long _moneyMinus = 200000;
    long _moneyPlus = 50000;
    public void SetCurrentMoney(long currentMoney)
    {
        _currentMoneyBarFillAnimation.SetFill((float)_currentMoney/_maxMoney);
        _currentMoney = currentMoney;
        _currentMoneyText.text = currentMoney.ToStringRupiahFormat();
        _currentMoneyBarFillAnimation.SetEndFill((float)currentMoney / _maxMoney).Play();

        _currentMoneyText_Finish.text = _currentMoneyText.text;
        _currentMoneyBarFillAnimation_Finish.SetFill((float)currentMoney / _maxMoney);
    }


    int _maxShotCount = 5;
    int _currentShotCount = 0;
    public void SetShotCount(int shotCount)
    {
        _shotCountBarFillAnimation.SetFill((float)(_maxShotCount-_currentShotCount)/(float)_maxShotCount);
        _currentShotCount = shotCount;
        _shotCountText.text = (_maxShotCount-_currentShotCount).ToString() + "/" + _maxShotCount;
        _shotCountBarFillAnimation.SetEndFill((float)(_maxShotCount-_currentShotCount) / (float)_maxShotCount).Play();

        _shotCountText_Finish.text = _shotCountText.text;
        _shotCountBarFillAnimation_Finish.SetFill((float)(_maxShotCount-_currentShotCount) / (float)_maxShotCount);
    }
    public override void Play()
    {
        for(int i = 0; i < _bungas.Length; i++)
        {
            int index = i;
            _bungas[i].OnShoot += (b) => OnBungaShot(b, index);
            _isBungaShot.Add(false);
        }

        _chosenBungaShooterData = _bungaShooterData[Random.Range(0, _bungaShooterData.Length)];
        _chosenBungaShooterData = Instantiate(_chosenBungaShooterData);
        for(int i = 0; i < _bungas.Length; i++)
        {
            _bungas[i].SetData(_chosenBungaShooterData.InitialBungaDatas[i].Percentage, _chosenBungaShooterData.InitialBungaDatas[i].Price);
        }

        _maxMoney = CalculateMinMoney() + _moneyPlus;
        _currentMoney = _maxMoney;

        _currentMoneyText.text = _maxMoney.ToStringRupiahFormat();
    }

    long CalculateMaxMoney()
    {
        List<(long, int)> bungas = new List<(long, int)>();
        for(int i = 0; i < _chosenBungaShooterData.InitialBungaDatas.Count; i++)
        {
            bungas.Add((_chosenBungaShooterData.InitialBungaDatas[i].Price, _chosenBungaShooterData.InitialBungaDatas[i].Percentage));
        }

        // sort based on price ascending
        bungas.Sort((a, b) => (a.Item1 * a.Item2).CompareTo((b.Item1*b.Item2)));

        long money = 0;
        // for(int i = bungas.Count-1; i >= 0; i--) {
        for(int i = 0; i < bungas.Count; i++) {
            float constantAdder = bungas[i].Item1 * ((bungas[i].Item2)/100f);
            long adder = (long)(bungas[i].Item1 + constantAdder * i);
            money += adder;
        }
        
        return money;
    }

    long CalculateMinMoney()
    {
        List<(long, int)> bungas = new List<(long, int)>();
        for(int i = 0; i < _chosenBungaShooterData.InitialBungaDatas.Count; i++)
        {
            bungas.Add((_chosenBungaShooterData.InitialBungaDatas[i].Price, _chosenBungaShooterData.InitialBungaDatas[i].Percentage));
        }

        // sort based on price descending
        bungas.Sort((a, b) => (a.Item1 * a.Item2).CompareTo((b.Item1*b.Item2)));
        bungas.Reverse();


        long money = 0;
        // for(int i = bungas.Count-1; i >= 0; i--) {
        for(int i = 0; i < bungas.Count; i++) {
            float constantAdder = bungas[i].Item1 * ((bungas[i].Item2)/100f);
            long adder = (long)(bungas[i].Item1 + constantAdder * i);
            money += adder;
        }
        
        return money;
    }


    void OnBungaShot(Bunga bunga, int index)
    {
        if(_currentMoney < _bungas[index].FinalPrice) {
            _bungas[index].Shake();
            _bungas[index].DisableAll();
            LoseCheck();
            
        } else {
            _isBungaShot[index] = true;
            _bungas[index].Throw();
            for(int i = 0; i < _isBungaShot.Count; i++)
            {
                if(!_isBungaShot[i])
                {
                    // _bungas[i].SetData(_bungas[i].Percentage+Random.Range(_minIncrement, _maxIncrement+1), _bungas[i].Price);
                    // _bungas[i].SetData(_bungas[i].Percentage+_maxIncrement, _bungas[i].Price);
                    _bungas[i].Increment();
                }
            }

            // UIs
            SetCurrentMoney(_currentMoney - bunga.FinalPrice);
            SetShotCount(_currentShotCount+1);
            WinCheck();
        }
    }


    [SerializeField] PopUp _finishPopUp;
    void WinCheck()
    {
        if(_currentShotCount == _maxShotCount) {
            _finishPopUp.transform.parent.gameObject.SetActive(true);
            _finishPopUp.Show();
            _popUpImg.sprite = _winPopUpSprite;
            HandleEnd();
        }
    }

    void LoseCheck()
    {
        // if current money less than all
        for(int i  = 0; i < _isBungaShot.Count; i++) {
            if(!_isBungaShot[i]) {
                if(_currentMoney > _bungas[i].FinalPrice) {
                    return;
                }
            }
        }


        this.Invoke(() => {
            _finishPopUp.transform.parent.gameObject.SetActive(true);
            _finishPopUp.Show();
            _popUpImg.sprite = _losePopUpSprite;
            HandleEnd();
        }, 1f);
    }


    void HandleEnd()
    {
        Save.Data.CurrentDayData.StreamingCounter++;
        Save.Data.CurrentDayData.GainedViews += _viewCounter*_currentShotCount;
        Save.Data.CurrentDayData.GainedSubscriber += (long)(_viewCounter*_currentShotCount * UnityEngine.Random.Range(1f, 1.5f));
        Save.Data.CurrentDayData.GainedMoney = (long)(_viewCounter*_currentShotCount * UnityEngine.Random.Range(150f, 200f));
        OnIncreaseViews(_viewCounter*_currentShotCount);
        MouseCursor.Main.SetToCursor();
    }

}
