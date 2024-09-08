using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfterStreaming : MonoBehaviour
{
    [SerializeField] SceneTransition _sceneTransition;
    [Header("Graph")]
    [SerializeField] UIGraph _viewsGraph;
    [SerializeField] UIGraph _subscriberGraph;
    [SerializeField] UIGraph _moneyGraph;
    
    [Header("Current Video")]
    [SerializeField] TextAnimation _penontonText;
    [SerializeField] TextAnimation _newSubscriberText;
    [SerializeField] MoneyAnimation _penghasilanText;

    [Header("Text")]
    [SerializeField] MoneyAnimation _willBeSalaryMoneyText;


    void OnEnable()
    {
        var viewsEachDay = Save.Data.ViewsEachDay; viewsEachDay.Insert(0, 0);
        var subscriberEachDay = Save.Data.SubscriberEachDay; subscriberEachDay.Insert(0, 0);
        var moneyEachDay = Save.Data.MoneyEachDay; moneyEachDay.Insert(0, 0);

        if(viewsEachDay.Count == 2) viewsEachDay[0] = viewsEachDay[1];
        if(subscriberEachDay.Count == 2) subscriberEachDay[0] = subscriberEachDay[1];
        if(moneyEachDay.Count == 2) moneyEachDay[0] = moneyEachDay[1];
        
        Debug.Log(viewsEachDay.Count);

        if(_viewsGraph.gameObject.activeInHierarchy) _viewsGraph.SetData(viewsEachDay);
        else _viewsGraph.SetDataNoAnimation(viewsEachDay);
        
        if(_subscriberGraph.gameObject.activeInHierarchy) _subscriberGraph.SetData(subscriberEachDay);
        else _subscriberGraph.SetDataNoAnimation(subscriberEachDay);

        if(_moneyGraph.gameObject.activeInHierarchy) _moneyGraph.SetData(moneyEachDay);
        else _moneyGraph.SetDataNoAnimation(moneyEachDay);

        _penontonText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedViews, 0.5f);
        _newSubscriberText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedSubscriber, 0.5f);
        _penghasilanText.SetAndAnimate(0, Save.Data.CurrentDayData.GainedMoney, 0.5f);


        _willBeSalaryMoneyText.SetAndAnimate(0, Save.Data.GetSalaryFromToday(), 0.5f);
    }


    public void ToBedroom()
    {
        _sceneTransition.StartSceneTransition("Bedroom");
    }
}
