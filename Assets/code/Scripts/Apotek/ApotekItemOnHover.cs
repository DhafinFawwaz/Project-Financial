using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApotekItemOnHover : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _price;
    [SerializeField] TextMeshProUGUI _health;
    [SerializeField] TextMeshProUGUI _stock;
    CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    ApotekManager.Medicine _medicine;
    public void SetData(ApotekManager.Medicine medicine)
    {
        _medicine = medicine;
        Refresh();
    }   

    public void Refresh()
    {
        _price.text = _medicine.Price.ToString();
        _health.text = _medicine.Health.ToString();
        _stock.text = _medicine.Stock.ToString();
    }

    public void Show()
    {
        StartCoroutine(TweenLocalScaleAnimation(transform, Vector3.one * 0.5f, Vector3.one, 0.15f, Ease.OutQuart));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 0, 1, 0.15f, Ease.OutQuart));
    }

    public void Hide()
    {
        StartCoroutine(TweenLocalScaleAnimation(transform, Vector3.one, Vector3.zero, 0.2f, Ease.InCubic));
        StartCoroutine(TweenCanvasGroupAlphaAnimation(_canvasGroup, 1, 0, 0.2f, Ease.InCubic));
    }
    byte _key = 0;
    IEnumerator TweenLocalScaleAnimation(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            trans.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key == requirement)
        {
            trans.localScale = end;
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
