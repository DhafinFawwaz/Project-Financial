using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualityBar : MonoBehaviour
{
    [SerializeField] TextAnimation _healthText;
    [SerializeField] TextAnimation _happinessText;
    [SerializeField] SlicedFilledImageFillAnimation _qualityBar;
    [SerializeField] Transform _leftAnchor;
    [SerializeField] Transform _rightAnchor;
    [SerializeField] Transform _iconTrans;
    [SerializeField] float _leftScale = 0.7f;
    [SerializeField] float _rightScale = 1f;

    public void SetAndAnimate(float quality, int health, int happiness, float initialHealth = 0, float initialHappiness = 0)
    {
        _qualityBar.SetEndFill(quality).Play();
        _healthText.SetAndAnimate(initialHealth, health, 0.15f);
        _happinessText.SetAndAnimate(initialHappiness, happiness, 0.15f);
    }

    public void SetNoAnimation(float quality, int health, int happiness)
    {
        _qualityBar.SetFill(quality);
        _qualityBar.Target.fillAmount = quality;
        _healthText.SetNoAnimation(health);
        _happinessText.SetNoAnimation(happiness);
    }

    void Update()
    {
        _iconTrans.position = Vector3.Lerp(_leftAnchor.position, _rightAnchor.position, _qualityBar.Target.fillAmount);
        _iconTrans.localScale = Vector3.Lerp(Vector3.one * _leftScale, Vector3.one * _rightScale, _qualityBar.Target.fillAmount);
    }

}
