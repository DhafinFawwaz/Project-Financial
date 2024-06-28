using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameChoose : MonoBehaviour
{
    float _itemWidth = 380;
    float _leftPadding = 80;
    float _layoutSpacing = 60;
    [SerializeField] HorizontalLayoutGroup _layoutGroup;
    [SerializeField] RectTransform _rt;

    int _currentPage = 0;
    float _initialX;
    public Action<int> OnGameSelected;

    void Start()
    {
        _itemWidth = _rt.GetChild(0).transform.GetComponent<RectTransform>().rect.width;
        _leftPadding = _layoutGroup.padding.left;
        _layoutSpacing = _layoutGroup.spacing;
        _initialX = GetComponent<RectTransform>().anchoredPosition.x;
    }

    [SerializeField] FiscalGuardianLoader fiscalGuardianLoader;
    void Update()
    {
        if(InputManager.GetKeyDown(KeyCode.RightArrow))
            NextPage();
        else if(InputManager.GetKeyDown(KeyCode.LeftArrow))
            PreviousPage();
        else if(InputManager.GetKeyDown(KeyCode.Return))
        {
            OnGameSelected?.Invoke(_currentPage);
            if(_currentPage == 0)
                fiscalGuardianLoader.Play();

        }
    }

    float _moveLength => _itemWidth + _layoutSpacing;
    Vector2 CalculateCurrentPosition() => new Vector2(_initialX - _moveLength * _currentPage, _rt.anchoredPosition.y);

    public void NextPage()
    {
        if (_currentPage < transform.childCount - 1)
        {
            _currentPage++;
            Refresh();
        }
    }

    public void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            Refresh();
        }
    }

    void Refresh()
    {
        StartCoroutine(TweenPositionAnimation(_rt, _rt.anchoredPosition, CalculateCurrentPosition(), 0.25f, Ease.OutQuart));
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if(i == _currentPage)
                child.GetComponent<StreamerGame>().Hightlight();
            else
                child.GetComponent<StreamerGame>().Unhightlight();
        }
    }
    

    byte _posKey = 0;
    IEnumerator TweenPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_posKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _posKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_posKey == requirement)
            rt.anchoredPosition = end;
    }


    byte _scaleKey = 0;
    IEnumerator TweenLocalScaleAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_scaleKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _scaleKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_scaleKey == requirement)
            rt.anchoredPosition = end;
    }
}
