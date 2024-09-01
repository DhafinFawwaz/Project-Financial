using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartItemSpawner : MonoBehaviour
{
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Transform[] _targetTransforms;
    [SerializeField] TransformAnimation[] _iconTransforms;
    float _spawnDelay = 0.1f;

    float[] _itemValues;
    void Start()
    {
        _itemValues = _pieChart.GetPieValues();
    }


    float _lastSpawnTime = 0;
    void Update()
    {
        if(Time.time - _lastSpawnTime < _spawnDelay) return;
        _lastSpawnTime = Time.time;

        float[] newItemValues = _pieChart.GetPieValues();

        int increasingIdx = -1;
        int decreasingIdx = -1;

        for(int i = 0; i < _itemValues.Length; i++)
        {
            if(newItemValues[i] > _itemValues[i])
            {
                _itemValues[i] = newItemValues[i];
                ItemTargetToPie(i);
                increasingIdx = i;
            }
            else if(newItemValues[i] < _itemValues[i])
            {
                _itemValues[i] = newItemValues[i];
                // ItemPieToTarget(i);
                decreasingIdx = i;
            }
        }

        if(increasingIdx != -1 && decreasingIdx != -1)
        {
            // TargetToTarget(decreasingIdx, increasingIdx);
            ItemPieToTarget(decreasingIdx, increasingIdx);
            // ItemTargetToPie(decreasingIdx);
        }
    }

    void ItemPieToTarget(int pieIdx, int targetIdx)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenPosition(item.transform, _pieChart.GetPieBaseCenterDirection(pieIdx), _targetTransforms[targetIdx].position, 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.1f);
        Destroy(item.gameObject, 0.45f);

        // Wallet scale animation
        this.Invoke(() => {
            _iconTransforms[targetIdx].transform.localScale = Vector3.one * 1.5f;
            _iconTransforms[targetIdx].TweenLocalScale();
        }, 0.3f);
    }

    void TargetToTarget(int idx1, int idx2)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenPosition(item.transform, _targetTransforms[idx1], _targetTransforms[idx2], 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.1f);
        Destroy(item.gameObject, 0.45f);
    }

    void ItemTargetToPie(int idx)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenTargetToPie(item.transform, idx, 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.1f);
        Destroy(item.gameObject, 0.45f);


        // Wallet scale animation
        this.Invoke(() => {
            _iconTransforms[idx].transform.localScale = Vector3.one * 1.5f;
            _iconTransforms[idx].TweenLocalScale();
        }, 0.3f);
    }

    void ItemPieToTarget(int idx)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenPieToTarget(item.transform, idx, 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.1f);
        Destroy(item.gameObject, 0.45f);
    }

    IEnumerator TweenPosition(Transform rt, Transform start, Transform end, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start.position, end.position, Ease.OutQuart(t));
            yield return null;
        }
    }
    IEnumerator TweenPosition(Transform rt, Vector3 start, Vector3 end, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            yield return null;
        }
    }
    IEnumerator TweenPieToTarget(Transform rt, int i, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        // float amplitude = Random.Range(-_amplitudeRange, _amplitudeRange);
        while (t <= 1)
        {
            Vector3 start = _pieChart.GetPieBaseCenterDirection(i);
            Vector3 end = _targetTransforms[i].position;
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            
            // Vector3 dir = (end - start).normalized;
            // Vector3 dir90 = new Vector3(dir.y, -dir.x, 0);
            // rt.position += dir90 * amplitude * Parabole(t);
            yield return null;
        }
    }

    [SerializeField] float _amplitudeRange = 5f;
    IEnumerator TweenTargetToPie(Transform rt, int i, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        // float amplitude = Random.Range(-_amplitudeRange, _amplitudeRange);
        while (t <= 1)
        {
            Vector3 start = _targetTransforms[i].position;
            Vector3 end = _pieChart.GetPieBaseCenterDirection(i);
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            
            // Vector3 dir = (end - start).normalized;
            // Vector3 dir90 = new Vector3(dir.y, -dir.x, 0);
            // rt.position += dir90 * amplitude * Parabole(t);

            yield return null;
        }
    }
    float Parabole(float x) => -4 * x * x + 4 * x;

    IEnumerator TweenScale(Transform rt, Vector3 start, Vector3 end, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localScale = Vector3.LerpUnclamped(start, end, Ease.InCubic(t));

            yield return null;
        }
    }
}
