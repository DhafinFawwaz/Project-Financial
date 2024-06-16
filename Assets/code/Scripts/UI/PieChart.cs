using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    [SerializeField] Image _bluePie;
    [SerializeField] Image _yellowPie;
    [SerializeField] Image _redPie;
    [SerializeField] Image _blueArrow;
    [SerializeField] Image _yellowArrow;
    [SerializeField] Image _redArrow;

    void Start()
    {
        _bluePie.fillAmount = 0;
        _yellowPie.fillAmount = 0;
        _redPie.fillAmount = 0;
        _blueArrow.transform.localEulerAngles = Vector3.zero;
        _yellowArrow.transform.localEulerAngles = Vector3.zero;
        _redArrow.transform.localEulerAngles = Vector3.zero;
    }

    public void SetAndAnimatePie(float blue, float yellow, float red)
    {
        StartCoroutine(Animation(_bluePie, _blueArrow, blue, 1f));
        StartCoroutine(Animation(_yellowPie, _yellowArrow, yellow, 1f));
        StartCoroutine(Animation(_redPie, _redArrow, 1, 1f));
    }

    IEnumerator Animation(Image pie, Image arrow, float target, float duration)
    {
        float start = pie.fillAmount;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            float pieFillAmount = Mathf.LerpUnclamped(start, target, Ease.OutQuart(t));
            pie.fillAmount = pieFillAmount;
            arrow.transform.localEulerAngles = new Vector3(0,0,pieFillAmount * -360);
            yield return null;
        }
        pie.fillAmount = target;
        arrow.fillAmount = target * -360;
    }
}
