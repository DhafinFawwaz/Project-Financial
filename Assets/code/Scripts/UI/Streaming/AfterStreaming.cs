using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterStreaming : MonoBehaviour
{
    [SerializeField] UIGraph _graph;
    [SerializeField] TextAnimation _penontonText;
    [SerializeField] TextAnimation _newSubscriberText;
    [SerializeField] MoneyAnimation _penghasilanText;
    [SerializeField] TextAnimation _totalSubscriberText;
    [SerializeField] ImageFillAnimation _fillAnimation;
    [SerializeField] SceneTransition _sceneTransition;

    public static long Penonton = 1000;
    public static long NewSubscriber = 100;
    public static long Penghasilan = 10;
    public static long TotalSubscriber = 50100;
    const int TARGET_SUBSCRIBER = 1000000;
    public static List<long> GainedSubscriberEachDay = new List<long>();

    void Start()
    {
        List<long> list = new List<long>();
        for(int i = 0; i < GainedSubscriberEachDay.Count; i++)
        {
            if(GainedSubscriberEachDay[i] != 0)
            {
                list.Add(GainedSubscriberEachDay[i]);
            }
        }
        list.Add(0);
        _graph.SetData(list);
        _penontonText.SetAndAnimate(0, Penonton, 0.5f);
        _newSubscriberText.SetAndAnimate(0, NewSubscriber, 0.5f);
        _penghasilanText.SetAndAnimate(0, Penghasilan, 0.5f);
        _totalSubscriberText.SetAndAnimate(0, TotalSubscriber, 0.5f);
        _fillAnimation.SetEndFill((float)TotalSubscriber / TARGET_SUBSCRIBER).Play();
        Save.Data.DayState = DayState.AfterStreaming;
    }


    public void ToBedroom()
    {
        _sceneTransition.StartSceneTransition("Bedroom");
    }
}
