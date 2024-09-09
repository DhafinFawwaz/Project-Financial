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
    [SerializeField] GameObject _upViews;
    [SerializeField] GameObject _downViews;
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Image _face;
    [SerializeField] GameChoose _gameChoose;
    [SerializeField] StreamingGames[] _games;
    [SerializeField] GameObject _home;


    int _viewsCounter = 0;
    void Awake()
    {
        _viewsCounter = Mathf.CeilToInt(Save.Data.SubscriberAmount/100);
        _views.SetAndAnimate(0, _viewsCounter, 0.5f);
        StartCoroutine(UpdatingFace());
    }


    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;
    void RefreshStats()
    {
        _healthText.text = Save.Data.Health.ToString("0");
        _happinessText.text = Save.Data.Happiness.ToString("0");
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



    void OnEnable()
    {
        _gameChoose.OnGameSelected += OnGameSelected;
    }

    void OnDisable()
    {
        _gameChoose.OnGameSelected -= OnGameSelected;
    }


    float _lastTime = 0;
    public void OnGameSelected(int page)
    {
        if(_games.Length <= page) return;

        if(Time.time - _lastTime < 0.5f) return;
        _lastTime = Time.time;
        Debug.Log("Game selected: " + page);

        StartCoroutine(StartGameAnimation(page));
    }

    [SerializeField] AnimationUI _blackScreenAnimation;
    IEnumerator StartGameAnimation(int page)    
    {
        _blackScreenAnimation.Play();
        yield return new WaitForSeconds(0.2f);
        var game = Instantiate(_games[page]);
        game.Play();
        game.OnIncreaseViews += OnIncreaseViews;
        game.OnDecreaseViews += OnDecreaseViews;
        game.SetViewCounter(_viewsCounter);
        game.OnGameEnd += OnGameEnd;

        _endStreamButton.SetButtonActive(false);

        Save.Data.Health -= game.HealthCost;
        Save.Data.Health -= game.HappinessCost;
        RefreshStats();
    }

    [SerializeField] AnimationUI _backToHomeAnimation;
    [SerializeField] AnimationUI _streamingEndAnimation;
    void OnGameEnd(StreamingGames game)
    {
        _endStreamButton.Refresh();
        _backToHomeAnimation.Play();
        Destroy(game.gameObject, 0.2f);

        if(Save.Data.CurrentDayData.StreamingCounter == Save.Data.MaxStreamingAmountPerDay)
        {
            _streamingEndAnimation.Play();
        }
    }

    void OnIncreaseViews(int newAmount)
    {
        _views.SetAndAnimate(_viewsCounter, newAmount, 0.5f);
        _viewsCounter = newAmount;
        _upViews.SetActive(true);
        _downViews.SetActive(false);
        HappyFace();
    }

    void OnDecreaseViews(int newAmount)
    {
        _views.SetAndAnimate(_viewsCounter, newAmount, 0.5f);
        _viewsCounter = newAmount;
        _upViews.SetActive(false);
        _downViews.SetActive(true);
        SadFace();
    }

    void DisableHome()
    {
        _home.SetActive(false);
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

    [SerializeField] EndStreamButton _endStreamButton;
}
