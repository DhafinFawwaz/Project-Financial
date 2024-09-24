using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfterStreamingAboutPage : MonoBehaviour
{
    [Header("Accumulated")]
    [SerializeField] TextAnimation _totalSubscriberText;
    [SerializeField] TextAnimation _totalViewsText;
    [SerializeField] TextAnimation _totalMoneyText;

    [Header("Animation")]
    [SerializeField] SlicedFilledImageFillAnimation _fillAnimation;

    [SerializeField] TextMeshProUGUI _targetSubscriberText;
    [SerializeField] long[] _targetSubscriber;
    [SerializeField] TransformAnimation[] _playButtonAnimations;

    void Awake()
    {
        _targetSubscriber[0] = SaveData.SILVER_PLAY_BUTTON;
        _targetSubscriber[1] = SaveData.GOLD_PLAY_BUTTON;
        _targetSubscriber[2] = SaveData.DIAMOND_PLAY_BUTTON;


        _targetSubscriber = new long[]{
            SaveData.SILVER_PLAY_BUTTON,
            SaveData.GOLD_PLAY_BUTTON,
            SaveData.DIAMOND_PLAY_BUTTON
        };
    }

    void OnEnable()
    {
        Save.Data.GetChannelInfo(out long totalSubscriber, out long totalViews, out long totalMoney, out long last3DaysMoney);

        _totalSubscriberText.SetAndAnimate(0, totalSubscriber, 0.5f);
        _totalViewsText.SetAndAnimate(0, totalViews, 0.5f);
        _totalMoneyText.SetAndAnimate(0, totalMoney, 0.5f);

        _playButtonAnimations[0].transform.localScale = Vector3.zero;
        _playButtonAnimations[1].transform.localScale = Vector3.zero;
        _playButtonAnimations[2].transform.localScale = Vector3.zero;

        _playButtonAnimations[0].SetEase(Ease.OutQuart);
        _playButtonAnimations[1].SetEase(Ease.OutQuart);
        _playButtonAnimations[2].SetEase(Ease.OutQuart);

        float duration = 0.5f;
        _fillAnimation.SetDuration(duration);
        
        _fillAnimation.SetFill(0);
        if(totalSubscriber < _targetSubscriber[0]) {
            _fillAnimation.SetEaseFunction(Ease.OutQuart).SetEndFill(Mathf.Clamp01((float)totalSubscriber / _targetSubscriber[0])).Play();
            _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[0].ToString("N0") + " Subscriber";
        } else {
            _fillAnimation.SetEaseFunction(Ease.Linear).SetEndFill(1).Play();
            _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[0].ToString("N0") + " Subscriber";

            this.Invoke(() => {
                _playButtonAnimations[0].gameObject.SetActive(true);
                _playButtonAnimations[0].TweenLocalScale();
                
                _fillAnimation.SetFill(0);
                if(totalSubscriber < _targetSubscriber[1]) {
                    _fillAnimation.SetEaseFunction(Ease.OutQuart).SetEndFill(Mathf.Clamp01((float)totalSubscriber / _targetSubscriber[1])).Play();
                    _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[1].ToString("N0") + " Subscriber";
                } else {
                    _fillAnimation.SetEaseFunction(Ease.Linear).SetEndFill(1).Play();
                    _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[1].ToString("N0") + " Subscriber";

                    this.Invoke(() => {
                        _playButtonAnimations[1].gameObject.SetActive(true);
                        _playButtonAnimations[1].TweenLocalScale();
                        
                        _fillAnimation.SetFill(0);
                        if(totalSubscriber < _targetSubscriber[2]) {
                            _fillAnimation.SetEaseFunction(Ease.OutQuart).SetEndFill(Mathf.Clamp01((float)totalSubscriber / _targetSubscriber[2])).Play();
                            _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[2].ToString("N0") + " Subscriber";
                        } else {
                            _fillAnimation.SetEaseFunction(Ease.Linear).SetEndFill(1).Play();
                            _targetSubscriberText.text = "Target: " + totalSubscriber.ToString("N0") + "/" + _targetSubscriber[2].ToString("N0") + " Subscriber";

                            this.Invoke(() => {
                                _playButtonAnimations[2].gameObject.SetActive(true);
                                _playButtonAnimations[2].TweenLocalScale();
                            }, duration);
                        }
                    }, duration);
                }

            }, duration);
        }

    }
}
