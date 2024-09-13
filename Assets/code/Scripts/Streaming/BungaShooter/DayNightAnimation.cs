using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightAnimation : MonoBehaviour
{
    [SerializeField] Material _day;
    [SerializeField] Material _night;
    [SerializeField] GameObject _dayBG;
    [SerializeField] GameObject _nightBG;
    [SerializeField] GameObject _dayFrontBG;
    public void Play()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        _dayBG.SetActive(true);
        _nightBG.SetActive(true);
        _dayFrontBG.SetActive(false);

        _night.SetFloat("_Progress", 0.5f);
        StartCoroutine(TweenMaterialFloat(_night, "_Progress", 1, 0.3f));
        yield return new WaitForSeconds(0.3f);
        
        _dayBG.SetActive(false);
        _nightBG.SetActive(true);
        _dayFrontBG.SetActive(true);


        _day.SetFloat("_Progress", 0.5f);
        StartCoroutine(TweenMaterialFloat(_day, "_Progress", 1, 0.3f));
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator TweenMaterialFloat(Material material, string propertyName, float targetValue, float duration)
    {
        float startValue = material.GetFloat(propertyName);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            material.SetFloat(propertyName, Mathf.Lerp(startValue, targetValue, time / duration));
            yield return null;
        }
        material.SetFloat(propertyName, targetValue);
    }
}
