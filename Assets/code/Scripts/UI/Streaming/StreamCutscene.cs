using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamCutscene : MonoBehaviour
{
    [SerializeField] Toaster _toaster;
    [SerializeField] Vector3[] _toasterPositions;
    [SerializeField] Vector3 _toasterThrowPosition;
    [SerializeField] Transform _toasterParent;

    List<Toaster> _toasters = new List<Toaster>();
    void Start()
    {
        
    }

    [ContextMenu("Test Toast")]
    public void TestToast()
    {
        SpawnToaster("Test", "Test test test");
    }

    float _tweenDuration = 2f;
    public void SpawnToaster(string title, string message)
    {
        Toaster toaster = Instantiate(_toaster, _toasterParent);
        toaster.SetText(title, message);
        RectTransform rt = toaster.GetComponent<RectTransform>();
        Vector2 pos = _toasterPositions[0];
        rt.anchoredPosition = new Vector2(-pos.x, pos.y);
        _toasters.Insert(0, toaster);
        MoveOtherToasters();
    }
    void MoveOtherToasters()
    {
        for(int i = 0; i < Mathf.Min(_toasterPositions.Length, _toasters.Count); i++)
        {
            RectTransform rt = _toasters[i].gameObject.GetComponent<RectTransform>();
            StartCoroutine(TweenAnchoredPositionAnimation(rt, rt.anchoredPosition, _toasterPositions[i], _tweenDuration, Ease.OutQuart));
        }
        if(_toasters.Count > _toasterPositions.Length) {
            RectTransform rt = _toasters[_toasterPositions.Length].gameObject.GetComponent<RectTransform>();
            _toasters.RemoveAt(_toasters.Count-1);
            StartCoroutine(TweenAnchoredPositionAnimation(rt, rt.anchoredPosition, _toasterThrowPosition, _tweenDuration, Ease.OutQuart, () => Destroy(rt.gameObject)));
        }

    }

    IEnumerator TweenAnchoredPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction, Action OnComplete = null)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        rt.anchoredPosition = end;
        OnComplete?.Invoke();
    }
}
