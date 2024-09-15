using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KTPBelanja : MonoBehaviour
{
    const string GRAY_TAG = "<color=#444444>";
    [SerializeField] TextMeshProUGUI _moneyTopText;
    [SerializeField] TextMeshProUGUI _moneyBottomText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;

    [SerializeField] RectTransform _money;
    [SerializeField] Image _greenBar;
    [SerializeField] Vector2 _moneyPositionLeft = new Vector2(0, 0);
    [SerializeField] Vector2 _moneyPositionRight = new Vector2(0, 0);

    [Range(0, 1)]
    [SerializeField] float _greenBarFillAmount = 0;

    void OnValidate()
    {
        _greenBar.fillAmount = _greenBarFillAmount;
        _money.anchoredPosition = Vector2.Lerp(_moneyPositionLeft, _moneyPositionRight, _greenBar.fillAmount);
    }

    public KTPBelanja SetMoneyTop(long money)
    {
        _moneyTopText.text = money.ToStringRupiahFormat();
        return this;
    }

    public KTPBelanja SetMoneyBottom(long money)
    {
        _moneyBottomText.text = money.ToStringRupiahFormat();
        return this;
    }

    bool _firstTime = true;    
    public KTPBelanja SetGreenBarFill(float percentage)
    {
        if(_firstTime)
            _greenBar.fillAmount = percentage;
        else if(gameObject.activeInHierarchy)
            StartCoroutine(TweenFillAmount(_greenBar, _greenBar.fillAmount, percentage, 0.15f));
        else
            _greenBar.fillAmount = percentage;
        _firstTime = false;
        
        _money.anchoredPosition = Vector2.Lerp(_moneyPositionLeft, _moneyPositionRight, _greenBar.fillAmount);
        return this;
    }
    IEnumerator TweenFillAmount(Image img, float from, float to, float duration)
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime/duration;
            img.fillAmount = Mathf.Lerp(from, to, Ease.OutQuart(t));
            _money.anchoredPosition = Vector2.Lerp(_moneyPositionLeft, _moneyPositionRight, _greenBar.fillAmount);
            yield return null;
        }
    }

    public KTPBelanja SetHapiness(double hapiness, double added)
    {
        _happinessText.text = hapiness.ToString();// + GRAY_TAG + "+" + added.ToString();
        return this;
    }

    public KTPBelanja SetHealth(double health, double added)
    {
        _healthText.text = health.ToString();// + GRAY_TAG + "+" + added.ToString();
        return this;
    }
}
