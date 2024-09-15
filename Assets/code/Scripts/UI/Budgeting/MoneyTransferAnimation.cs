using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTransferAnimation : MonoBehaviour
{
    [SerializeField] Transform _moneyPrefab;
    [SerializeField] float _amplitudeRange = 100f;
    [SerializeField] float _targetRadiusRange = 100f;
    [SerializeField] float _duration = 1f;
    [SerializeField] float _transferDuration = 0.3f;
    [SerializeField] float _delayEachSpawn = 0.02f;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _targetPos;
    [SerializeField] TransformAnimation _startScaleAnimation;
    [SerializeField] TransformAnimation _targetScaleAnimation;

    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(Animation());
    }

    Vector3 GetRandomTargetPosition()
    {
        return _targetPos.position + Random.insideUnitSphere * _targetRadiusRange;
    }


    public static System.Action s_OnMoneySpawn;
    void SpawnMoney()
    {
        s_OnMoneySpawn?.Invoke();
        var money = Instantiate(_moneyPrefab, _startPos.position, Quaternion.identity, transform.parent);
        
        _startScaleAnimation.transform.localScale = Vector3.one * 1.2f;
        _startScaleAnimation.TweenLocalScale();

        StartCoroutine(TweenPosition(money, _startPos.position, GetRandomTargetPosition(), _transferDuration));
        this.Invoke(() => {
            StartCoroutine(TweenScale(money, _startScaleAnimation.transform.localScale, Vector3.zero, _transferDuration));
        }, _transferDuration * 0.5f);


        this.Invoke(() => {
            _targetScaleAnimation.transform.localScale = Vector3.one * 1.2f;
            _targetScaleAnimation.TweenLocalScale();
        }, _transferDuration * 0.9f);

        Destroy(money.gameObject, _transferDuration + 0.5f);
    }

    IEnumerator Animation()
    {
        float t = 0;
        float lastSpawnTime = 0;
        lastSpawnTime = Time.time;
        while(t < _duration)
        {
            t += Time.deltaTime;
            if(Time.time - lastSpawnTime > _delayEachSpawn)
            {
                lastSpawnTime = Time.time;
                SpawnMoney();
            }
            yield return null;
        }
    }


    IEnumerator TweenPosition(Transform rt, Vector3 start, Vector3 end, float duration)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        float amplitude = Random.Range(-_amplitudeRange, _amplitudeRange);
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, Ease.Linear(t));
            
            Vector3 dir = (end - start).normalized;
            Vector3 dir90 = new Vector3(dir.y, -dir.x, 0);
            rt.position += dir90 * amplitude * Parabole(t);
            yield return null;
        }
        rt.position = end;
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
        rt.localScale = end;
    }

    float Parabole(float x) => -4 * x * x + 4 * x;

}
