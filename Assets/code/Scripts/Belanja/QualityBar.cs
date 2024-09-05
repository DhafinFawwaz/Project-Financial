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

    public void SetAndAnimate(float quality, int health, int happiness, float initialHealth = 0, float initialHappiness = 0)
    {
        _qualityBar.SetEndFill(quality).Play();
        _healthText.SetAndAnimate(initialHealth, health, 0.15f);
        _happinessText.SetAndAnimate(initialHappiness, happiness, 0.15f);
    }

    public void SetNoAnimation(float quality, int health, int happiness)
    {
        _qualityBar.SetFill(quality);
        _healthText.SetNoAnimation(health);
        _happinessText.SetNoAnimation(happiness);
    }

}
