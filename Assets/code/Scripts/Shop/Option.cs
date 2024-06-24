using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField] GameObject _diskonGO;
    [SerializeField] GameObject _inflasiGO;
    [SerializeField] GameObject _buy1get1GO;
    [SerializeField] GameObject _up;
    [SerializeField] GameObject _down;
    [SerializeField] TextMeshProUGUI _buyCountText;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;
    OptionContent _optionContent;
    int _buyCount = 0;
    public int BuyCount => _buyCount;

    public void SetValues(OptionContent option)
    {
        _optionContent = option;
        Refresh();
    }
    public void IncrementBuyCount()
    {
        _buyCount++;
        _buyCountText.text = "x"+_buyCount.ToString();
        StartCoroutine(TweenLocalScaleAnimation(transform as RectTransform, transform.localScale, Vector3.one*1.2f, 0.15f, Ease.OutBackCubic));
    }
    public void ResetBuyCount()
    {
        _buyCount = 0;
        _buyCountText.text = "x"+_buyCount.ToString();
        StartCoroutine(TweenLocalScaleAnimation(transform as RectTransform, transform.localScale, Vector3.one, 0.15f, Ease.OutQuart));
    }

    public void Refresh()
    {
        _diskonGO.SetActive(_optionContent.Diskon);
        _inflasiGO.SetActive(_optionContent.Inflasi);
        _buy1get1GO.SetActive(_optionContent.Buy1get1);
        _buyCountText.text = "x"+_buyCount.ToString();
        _priceText.text = "Rp"+_optionContent.Price.ToStringRupiahFormat();
        _healthText.text = _optionContent.Health.ToString();
        _happinessText.text = _optionContent.Happiness.ToString();
        if(_optionContent.IsUp)
        {
            _up.SetActive(true);
            _down.SetActive(false);
        }
        else
        {
            _up.SetActive(false);
            _down.SetActive(true);
        }
    }

    public void Hide()
    {
        StartCoroutine(TweenLocalScaleAnimation(transform as RectTransform, Vector3.one, Vector3.zero, 0.2f, Ease.InCubic));
        Destroy(gameObject, 0.2f);
    }
    public void Show()
    {
        StartCoroutine(TweenLocalScaleAnimation(transform as RectTransform, Vector3.zero, Vector3.one, 0.2f, Ease.OutQuart));
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
            rt.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_scaleKey == requirement)
            rt.localScale = end;
    }
}
