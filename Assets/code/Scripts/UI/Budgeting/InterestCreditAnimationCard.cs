using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterestCreditAnimationCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _deadLineText;
    [SerializeField] TextMeshProUGUI _dayText;
    [SerializeField] TextMeshProUGUI _creditMoneyText;
    [SerializeField] TextRupiahAnimation _creditMoneyAnimation;
    [SerializeField] TransformAnimation _transformAnimation;

    public TextMeshProUGUI DeadLineText => _deadLineText;
    public TextMeshProUGUI DayText => _dayText;
    public TextMeshProUGUI CreditMoneyText => _creditMoneyText;
    public TextRupiahAnimation CreditMoneyAnimation => _creditMoneyAnimation;
    public TransformAnimation TransformAnimation => _transformAnimation;

    long _creditMoney;
    public void Set(int deadline, int day, long creditMoney)
    {   
        _creditMoney = creditMoney;
        _deadLineText.text = deadline.ToString();
        _dayText.text = "Tagihan Hari ke-" + day.ToString();
        _creditMoneyText.text = creditMoney.ToStringRupiahFormat();
    }

    public void PlayCreditMoneyTo(long toCreditMoney)
    {
        _creditMoneyAnimation.SetValues(_creditMoney, toCreditMoney);
        _creditMoneyAnimation.Play();
    }


}
