using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class TextAnimation : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;
    protected string _prefix = "";

    public void SetAndAnimate(float initialValue, float finalValue, float duration)
    {
        StartCoroutine(Animate(initialValue, finalValue, duration));
    }

    IEnumerator Animate(float initialValue, float finalValue, float duration)
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime/duration;
            _text.text = getFormattedValue(Mathf.Lerp(initialValue, finalValue, Ease.OutQuad(t)));
            yield return null;
        }
        _text.text = getFormattedValue(finalValue);
    }

    protected virtual string getFormattedValue(float value)
    {
        return _prefix + Mathf.RoundToInt(value).ToString();
    }

    public virtual void SetPrefix(string prefix)
    {
        _prefix = prefix;
    }

    void Reset()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
}
