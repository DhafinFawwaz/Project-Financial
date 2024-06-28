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

    public KTPWorld SetHealth(double health) 
    {
        _healthText.text = health.ToString();
        _redBar.fillAmount = (float)health/100;
        _healthIcon.anchoredPosition = Vector2.Lerp(_healthPositionLeft, _healthPositionRight, _redBar.fillAmount);
        return this;
    }

    public KTPWorld SetHappiness(double happiness)
    {
        _happinessText.text = happiness.ToString();
        _yellowBar.fillAmount = (float)happiness/100;
        _happinessIcon.anchoredPosition = Vector2.Lerp(_happinessPositionLeft, _happinessPositionRight, _yellowBar.fillAmount);
        return this;
    }

    public KTPWorld SetMoney(long money)
    {
        _moneyText.text = money.ToString();
        return this;
    }

    public KTPWorld SetSkillPoint(long point)
    {
        _skillPointText.text = point.ToString();
        return this;
    }
}
