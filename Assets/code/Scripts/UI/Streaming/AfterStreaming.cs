using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfterStreaming : MonoBehaviour
{
    [Header("Graph")]
    [SerializeField] UIGraph _viewsGraph;
    [SerializeField] UIGraph _subscriberGraph;
    [SerializeField] UIGraph _moneyGraph;
    
    [Header("Current Video")]
    [SerializeField] TextAnimation _penontonText;
    [SerializeField] TextAnimation _newSubscriberText;
    [SerializeField] MoneyAnimation _penghasilanText;

    [Header("Accumulated")]
    [SerializeField] TextAnimation _totalSubscriberText;
    [SerializeField] TextAnimation _totalViewsText;
    [SerializeField] TextAnimation _totalMoneyText;

    [Header("Animation")]
    [SerializeField] ImageFillAnimation _fillAnimation;
    [SerializeField] SceneTransition _sceneTransition;

    const int DIAMOND_PLAY_BUTTON_MINIMUM_SUBSCRIBER = 1000000;
    public static List<long> GainedSubscriberEachDay = new List<long>();


    void Start()
    {
        _viewsGraph.SetData(Save.Data.ViewsEachDay);
        _subscriberGraph.SetData(Save.Data.SubscriberEachDay);
        _moneyGraph.SetData(Save.Data.MoneyEachDay);


        _penontonText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedViews, 0.5f);
        _newSubscriberText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedSubscriber, 0.5f);
        _penghasilanText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedMoney, 0.5f);


        Save.Data.GetChannelInfo(out long totalSubscriber, out long totalViews, out long totalMoney, out long last3DaysMoney);

        _totalSubscriberText.SetAndAnimate(0, totalSubscriber, 0.5f);
        _totalViewsText.SetAndAnimate(0, totalViews, 0.5f);
        _totalMoneyText.SetAndAnimate(0, totalMoney, 0.5f);


        _fillAnimation.SetEndFill((float)totalSubscriber / DIAMOND_PLAY_BUTTON_MINIMUM_SUBSCRIBER).Play();
    }


    public void ToBedroom()
    {
        _sceneTransition.StartSceneTransition("Bedroom");
    }
}
