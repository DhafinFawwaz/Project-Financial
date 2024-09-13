using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterestManager : MonoBehaviour
{
    [SerializeField] TextRupiahAnimation _moneyTransferAnimation;
    [SerializeField] InterestCreditAnimationCard _interestCreditAnimationCard;
    [SerializeField] Transform _interestCardParent;
    [SerializeField] TransformAnimation _transformAnimation;

    void Awake()
    {
        _moneyTransferAnimation.SetNoAnimation(Save.Data.DebitMoney);
    }

    public void PlayDebitInterest()
    {
        _moneyTransferAnimation.SetValues(Save.Data.DebitMoney, Save.Data.DebitMoney * (1+SaveData.INTEREST_RATE));
        _moneyTransferAnimation.Play();
        Save.Data.DebitMoney = (long)(Save.Data.DebitMoney * (1+SaveData.INTEREST_RATE));
    }

    public void PlayCreditInterest()
    {
        StartCoroutine(PlayAnimation());
    }

    [SerializeField] UnityEvent _onAnimationEnd;

    IEnumerator PlayAnimation()
    {
        Save.Data.DebitTabunganMoney += Save.Data.DebitMoney;
        Save.Data.DebitMoney = 0;
        
        List<long> credits = new();
        List<int> days = new();
        List<int> deadlines = new();

        for (int i = 0; i < Save.Data.DayDatas.Count; i++) {
            if(Save.Data.DayDatas[i].CreditMoney > 0 && i + 3 == Save.Data.CurrentDay) { // Todo, check if its already 3 days
                credits.Add(Save.Data.DayDatas[i].CreditMoney);
                days.Add(i);
                deadlines.Add(i + SaveData.KREDIT_DEADLINE);
            }
        }

        List<InterestCreditAnimationCard> cards = new();
        for (int i = 0; i < credits.Count; i++) {
            InterestCreditAnimationCard card = Instantiate(_interestCreditAnimationCard, _interestCardParent);
            cards.Add(card);
        }

        for (int i = 0; i < credits.Count; i++) {
            var card = cards[i];
            card.TransformAnimation.TweenLocalScale();
            card.Set(deadlines[i], days[i]+1, credits[i]);
            yield return new WaitForSeconds(0.1f);
        }


        for (int i = 0; i < credits.Count; i++) {
            var card = cards[i];
            credits[i] = (long)(credits[i] * (1+SaveData.INTEREST_RATE));
            
            card.PlayCreditMoneyTo(credits[i]);
            Save.Data.DayDatas[days[i]].CreditMoney = credits[i];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < credits.Count; i++) {
            yield return new WaitForSeconds(0.1f);
            var card = cards[i];
            card.TransformAnimation.SetEnd(Vector3.zero);
            card.TransformAnimation.TweenLocalScale();
        }

        _transformAnimation.SetEnd(Vector3.zero);
        _transformAnimation.TweenLocalScale();
        yield return new WaitForSeconds(0.5f);
        _onAnimationEnd?.Invoke();
    }
}
