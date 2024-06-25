using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Kuliah : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _counterText;
    [SerializeField] RectTransform _wheel;
    [SerializeField] int _minRot = 3;
    [SerializeField] int _maxRot = 8;
    [SerializeField] float _tolerance = 10;
    [SerializeField] UnityEvent _onSpinHeartEnd;
    [SerializeField] UnityEvent _onSpinUnguEnd;
    [SerializeField] UnityEvent _onDone;
    [SerializeField] AnimationUI[] _animationUIs;

    void Start()
    {
        _counterText.text = "x2";
    }

    int _counter = 0;
    public void NextSpin()
    {
        if(_counter == 0) {
            _counterText.text = "x1";
            StartSpinWheelHeart();
        }
        else if(_counter == 1) {
            _counterText.text = "x0";
            StartSpinWheelUngu();
        }
        _counter++;
    }

    [ContextMenu("Spin Heart")]
    public void StartSpinWheelHeart()
    {
        int randSide = Random.Range(0, 4);
        float randRot = 0;

        if(randSide == 0) randRot = Random.Range(-90f + _tolerance, -45f - _tolerance);
        else if(randSide == 1) randRot = Random.Range(-180f + _tolerance, -135f - _tolerance);
        else if(randSide == 2) randRot = Random.Range(-270f + _tolerance, -225f - _tolerance);
        else if(randSide == 3) randRot = Random.Range(-360f + _tolerance, -315f - _tolerance);

        int randRotAmount = Random.Range(_minRot, _maxRot);
        StartCoroutine(TaskLocalEulerAngles(_wheel, _wheel.localEulerAngles, new Vector3(0, 0, randRot + randRotAmount*360), 4, Ease.InOutQuad));
        _onSpinHeartEnd.Invoke();
        StartCoroutine(DelayAnimation(_animationUIs[Random.Range(0, 2)], 4));
    }

    public void StartSpinWheelUngu()
    {
        int randSide = Random.Range(0, 4);
        float randRot = 0;

        if(randSide == 0) randRot = Random.Range(-45f + _tolerance, 0f - _tolerance);
        else if(randSide == 1) randRot = Random.Range(-135f + _tolerance, -90f - _tolerance);
        else if(randSide == 2) randRot = Random.Range(-225f + _tolerance, -180f - _tolerance);
        else if(randSide == 3) randRot = Random.Range(-315f + _tolerance, -270f - _tolerance);

        int randRotAmount = Random.Range(_minRot, _maxRot);
        StartCoroutine(TaskLocalEulerAngles(_wheel, _wheel.localEulerAngles, new Vector3(0, 0, randRot + randRotAmount*360), 4, Ease.InOutQuad));
        _onSpinUnguEnd.Invoke();
        StartCoroutine(DelayAnimation(_animationUIs[Random.Range(2, 4)], 4));

        StartCoroutine(DelayDone(6));
    }

    IEnumerator DelayAnimation(AnimationUI anim, float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.Play();
    }

    IEnumerator DelayDone(float delay)
    {
        yield return new WaitForSeconds(delay);
        _onDone?.Invoke();
    }

    IEnumerator TaskLocalEulerAngles(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localEulerAngles = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        rt.localEulerAngles = end;
    }
}
