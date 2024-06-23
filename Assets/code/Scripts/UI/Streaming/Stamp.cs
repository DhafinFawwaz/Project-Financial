using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    [SerializeField] RectTransform _dropArea;
    [SerializeField] RectTransform _dragableStamp;
    Vector3 _initialPosition;
    [SerializeField] ButtonUI _buttonUI;
    [SerializeField] GameObject _toActivate;
    public Action OnStamped;

    void Start()
    {
        _initialPosition = _dragableStamp.anchoredPosition;
    }
    void OnEnable()
    {
        _buttonUI.OnPointerDownEvent.AddListener(OnPointerDown);
        _buttonUI.OnPointerUpEvent.AddListener(OnPointerUp);
    }
    void OnDisable()
    {
        _buttonUI.OnPointerDownEvent.AddListener(OnPointerDown);
        _buttonUI.OnPointerUpEvent.AddListener(OnPointerUp);
    }

    void OnPointerDown()
    {
        _posKey++;
        _isDragging = true;
        _dragableStamp.SetAsLastSibling();
    }

    void OnPointerUp()
    {
        _isDragging = false;
        if(Vector3.Distance(_dragableStamp.position, _dropArea.position) < 100)
        {
            _toActivate.gameObject.SetActive(true);
            OnStamped?.Invoke();
        }
        else
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        StartCoroutine(TweenAnchoredPositionAnimation(_dragableStamp, _dragableStamp.anchoredPosition, _initialPosition, 0.3f, Ease.OutQuad));

    }

    public void OnDrag()
    {
        _dragableStamp.position = Input.mousePosition;
    }

    bool _isDragging = false;
    void Update()
    {
        if(_isDragging)
        {
            _dragableStamp.position = Input.mousePosition;
        }
    }


    byte _posKey = 0;
    IEnumerator TweenAnchoredPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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
}
