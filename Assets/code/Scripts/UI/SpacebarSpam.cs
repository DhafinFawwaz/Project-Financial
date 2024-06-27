using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpacebarSpam : MonoBehaviour
{
    [SerializeField] Image _bar;
    [SerializeField] int _clickAmount = 100;
    int _clickCount;
    [SerializeField] UnityEvent _onComplete;
    public void Play()
    {
        _clickCount = 0;
        gameObject.SetActive(true);
    }
    public void Stop()
    {
        gameObject.SetActive(false);
        _bar.fillAmount = 0;
        _onComplete?.Invoke();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _clickCount++;
            _bar.fillAmount = (float)_clickCount / _clickAmount;
        }
        if(_clickCount >= _clickAmount)
        {
            Stop();
        }
    }
}
