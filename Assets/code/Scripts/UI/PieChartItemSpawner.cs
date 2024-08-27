using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartItemSpawner : MonoBehaviour
{
    [SerializeField] PieChartDraggable _pieChart;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Transform[] _targetTransforms;
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

        for(int i = 0; i < _itemValues.Length; i++)
        {
            if(newItemValues[i] > _itemValues[i])
            {
                _itemValues[i] = newItemValues[i];
                ItemTargetToPie(i);
            }
            else if(newItemValues[i] < _itemValues[i])
            {
                _itemValues[i] = newItemValues[i];
                ItemPieToTarget(i);
            }
        }
    }

    void ItemTargetToPie(int idx)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenTargetToPie(item.transform, idx, 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.1f);
        Destroy(item.gameObject, 0.45f);
    }

    void ItemPieToTarget(int idx)
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(TweenPieToTarget(item.transform, idx, 0.3f));
        this.Invoke(() => {
            StartCoroutine(TweenScale(item.transform, Vector3.one, Vector3.zero, 0.2f));
        }, 0.2f);
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
    IEnumerator TweenPieToTarget(Transform rt, int i, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            Vector3 start = _pieChart.GetPieBaseCenterDirection(i);
            Vector3 end = _targetTransforms[i].position;
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            yield return null;
        }
    }

    IEnumerator TweenTargetToPie(Transform rt, int i, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            Vector3 start = _targetTransforms[i].position;
            Vector3 end = _pieChart.GetPieBaseCenterDirection(i);
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            yield return null;
        }
    }

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
