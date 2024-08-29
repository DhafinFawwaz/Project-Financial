using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApotekItemOnHover : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _price;
    [SerializeField] TextMeshProUGUI _health;
    RectTransform _rt;
    CanvasGroup _canvasGroup;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public void SetData(ApotekManager.Medicine medicine)
    {
        _price.text = medicine.Price.ToString();
        _health.text = medicine.Health.ToString();
    }   

    public void Show()
    {
        StartCoroutine(TweenLocalScaleAnimation(_rt, Vector3.one * 0.5f, Vector3.one, 0.15f, Ease.OutQuart));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 0, 1, 0.15f, Ease.OutQuart));
    }

    public void Hide()
    {
        StartCoroutine(TweenLocalScaleAnimation(_rt, Vector3.one, Vector3.zero, 0.2f, Ease.InCubic));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 1, 0, 0.2f, Ease.InCubic));
    }
    byte _key = 0;
    IEnumerator TweenLocalScaleAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key == requirement)
        {
            rt.localScale = end;
        }
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
}
