using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KTPWorld : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _skillPointText;


    [Header("Views")]
    [SerializeField] Image _redBar;
    [SerializeField] RectTransform _healthIcon;
    [SerializeField] Vector2 _healthPositionLeft = new Vector2(0, 0);
    [SerializeField] Vector2 _healthPositionRight = new Vector2(0, 0);

    [SerializeField] Image _yellowBar;
    [SerializeField] RectTransform _happinessIcon;
    [SerializeField] Vector2 _happinessPositionLeft = new Vector2(0, 0);
    [SerializeField] Vector2 _happinessPositionRight = new Vector2(0, 0);
    

    [Range(0, 1)]
    [SerializeField] float _redBarFillAmount = 0;
    [Range(0, 1)]
    [SerializeField] float _yellowBarFillAmount = 0;

    void OnValidate()
    {
        _redBar.fillAmount = _redBarFillAmount;
        _healthIcon.anchoredPosition = Vector2.Lerp(_healthPositionLeft, _healthPositionRight, _redBar.fillAmount);

        _yellowBar.fillAmount = _yellowBarFillAmount;
        _happinessIcon.anchoredPosition = Vector2.Lerp(_happinessPositionLeft, _happinessPositionRight, _yellowBar.fillAmount);
    }

    bool _firstTimeHealth = true;
    public KTPWorld SetHealth(double health) 
    {
        _healthText.text = health.ToString();
     
        if(_firstTimeHealth)
            _redBar.fillAmount = (float)health/100;
        else if(gameObject.activeInHierarchy)
            StartCoroutine(TweenFillAmount(_redBar, _redBar.fillAmount, (float)health/100, 0.15f));
        else
            _redBar.fillAmount = (float)health/100;
        _firstTimeHealth = false;

        _healthIcon.anchoredPosition = Vector2.Lerp(_healthPositionLeft, _healthPositionRight, _redBar.fillAmount);
        return this;
    }

    bool _firstTimeHappiness = true;
    public KTPWorld SetHappiness(double happiness)
    {
        _happinessText.text = happiness.ToString();
        
        if(_firstTimeHappiness)
            _yellowBar.fillAmount = (float)happiness/100;
        else if(gameObject.activeInHierarchy)
            StartCoroutine(TweenFillAmount(_yellowBar, _yellowBar.fillAmount, (float)happiness/100, 0.15f));
        else 
            _yellowBar.fillAmount = (float)happiness/100;
        _firstTimeHappiness = false;

        _happinessIcon.anchoredPosition = Vector2.Lerp(_happinessPositionLeft, _happinessPositionRight, _yellowBar.fillAmount);
        return this;
    }

    public KTPWorld SetMoney(long money)
    {
        _moneyText.text = money.ToStringCurrencyFormat();
        return this;
    }

    public KTPWorld SetSkillPoint(long point)
    {
        _skillPointText.text = point.ToString();
        return this;
    }

    IEnumerator TweenFillAmount(Image img, float from, float to, float duration)
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime/duration;
            img.fillAmount = Mathf.Lerp(from, to, Ease.OutQuart(t));
            _healthIcon.anchoredPosition = Vector2.Lerp(_healthPositionLeft, _healthPositionRight, _redBar.fillAmount);
            _happinessIcon.anchoredPosition = Vector2.Lerp(_happinessPositionLeft, _happinessPositionRight, _yellowBar.fillAmount);
            yield return null;
        }
    }
}
