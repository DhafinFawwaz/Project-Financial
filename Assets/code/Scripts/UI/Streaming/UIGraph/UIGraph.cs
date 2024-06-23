using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGraph : MonoBehaviour
{
    [SerializeField] RectTransform _linePrefab;
    [SerializeField] RectTransform _dotPrefab;
    List<RectTransform> _objects = new List<RectTransform>();
    [SerializeField] RectTransform _area;
    public void SetData(List<int> data)
    {
        // Clear old data
        foreach (var obj in _objects)
        {
            if(obj != null)
            Destroy(obj.gameObject);
        }
        _objects.Clear();

        float width = _area.rect.width;
        float height = _area.rect.height;

        // Find max value
        int max = 0;
        foreach (var d in data)
            if (d > max) max = d;

        // Create line
        for (int i = 0; i < data.Count-1; i++)
        {
            float dx = width / (data.Count - 1);
            float dy = (data[i + 1] - data[i]) * height / max;
            float c = Mathf.Sqrt(dx * dx + dy * dy);
            float angle = Mathf.Asin(dy / c) * Mathf.Rad2Deg;


            var line = Instantiate(_linePrefab, _area);
            line.anchoredPosition = new Vector2(i * width / (data.Count - 1), 0);
            line.sizeDelta = new Vector2(dx, line.sizeDelta.y);
            line.localEulerAngles = new Vector3(0, 0, 0);


            // line.anchoredPosition = new Vector2(i * width / (data.Count - 1), data[i] * height / max);
            // line.sizeDelta = new Vector2(c, line.sizeDelta.y);
            // line.localEulerAngles = new Vector3(0, 0, angle);

            StartCoroutine(TweenAnchoredPositionAnimation(line, line.anchoredPosition, new Vector2(i * width / (data.Count - 1), data[i] * height / max), 1, Ease.OutQuart));
            StartCoroutine(TweenSizeDelteAnimation(line, line.sizeDelta, new Vector2(c, line.sizeDelta.y), 1, Ease.OutQuart));
            StartCoroutine(TweenEulerAnglesAnimation(line, line.localEulerAngles, new Vector3(0, 0, angle), 1, Ease.OutQuart));
            _objects.Add(line);
        }
        

        // Create dot
        for (int i = 0; i < data.Count; i++)
        {
            var dot = Instantiate(_dotPrefab, _area);
            dot.anchoredPosition = new Vector2(i * width / (data.Count - 1), 0);

            // dot.anchoredPosition = new Vector2(i * width / (data.Count - 1), data[i] * height / max);
            StartCoroutine(TweenAnchoredPositionAnimation(dot, dot.anchoredPosition, new Vector2(i * width / (data.Count - 1), data[i] * height / max), 1, Ease.OutQuart));
            _objects.Add(dot);
        }
    }




    IEnumerator TweenEulerAnglesAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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


    IEnumerator TweenAnchoredPositionAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        rt.anchoredPosition = end;
    }


    IEnumerator TweenSizeDelteAnimation(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.sizeDelta = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        rt.sizeDelta = end;
    }
}
