using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterStreaming : MonoBehaviour
{
    [SerializeField] UIGraph _graph;
    [SerializeField] TextAnimation _penontonText;
    [SerializeField] TextAnimation _newSubscriberText;
    [SerializeField] TextAnimation _penghasilanText;
    [SerializeField] TextAnimation _totalSubscriberText;
    [SerializeField] ImageFillAnimation _fillAnimation;
    [SerializeField] SceneTransition _sceneTransition;

    public static int Penonton = 1000;
    public static int NewSubscriber = 100;
    public static int Penghasilan = 10;
    public static int TotalSubscriber = 50100;
    const int TARGET_SUBSCRIBER = 1000000;

    void Start()
    {
        _graph.SetData(new List<int> { 100, 130, 50, 30, 70, 10, 100, 100, 50, 40 });
        _penontonText.SetAndAnimate(0, Penonton, 0.5f);
        _newSubscriberText.SetAndAnimate(0, NewSubscriber, 0.5f);
        _penghasilanText.SetAndAnimate(0, Penghasilan, 0.5f);
        _totalSubscriberText.SetAndAnimate(0, TotalSubscriber, 0.5f);
        _fillAnimation.SetEndFill((float)TotalSubscriber / TARGET_SUBSCRIBER).Play();
    }


    public void ToBedroom()
    {
        _sceneTransition.StartSceneTransition("Bedroom");
    }
}
