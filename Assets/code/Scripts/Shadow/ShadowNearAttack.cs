using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public class ShadowNearAttack : MonoBehaviour
{
    [SerializeField] PlayerCore _player;
    [SerializeField] Material _circleRadiusMat;
    [SerializeField] Transform _circleTrans;
    [SerializeField] float _radius = 5;
    [SerializeField] bool _showGizmos = true;

    void OnDrawGizmos()
    {
        if (!_showGizmos) return;
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, _radius);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _radius);
    }

    void OnValidate()
    {
        _circleTrans.localScale = Vector3.one * _radius * 2;
    }


    void SetMatTransparency(float alpha)
    {
        StartCoroutine(TweenTransparency(alpha));
    }

    byte _key;
    IEnumerator TweenTransparency(float alpha, float duration = 0.3f)
    {
        float t = 0;
        byte requirements = ++ _key;
        float start = _circleRadiusMat.GetFloat("_Transparency");
        float end = alpha;
        while(t <= 1 && requirements == _key)
        {
            t += Time.deltaTime/duration;
            _circleRadiusMat.SetFloat("_Transparency", Mathf.Lerp(start, end, Ease.OutQuart(t)));
            yield return null;
        }
        if(requirements == _key) {
            _circleRadiusMat.SetFloat("_Transparency", end);
        }
    }


    void Start()
    {
        _player = PlayerCore.Instance;
        _circleRadiusMat.SetFloat("_Transparency", 0);
    }


    bool _isPlayerInRadius = false;
    void Update()
    {
        if(!enabled) return;
        if(_player == null) _player = PlayerCore.Instance;
        if(_player == null) return;

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        if(!_isPlayerInRadius && distance <= _radius)
        {
            _isPlayerInRadius = true;
            SetMatTransparency(0.5f);
            StartCoroutine(TimerCountdown());
        }
        else if(_isPlayerInRadius && distance > _radius)
        {
            _isPlayerInRadius = false;
            SetMatTransparency(0);
            CancelTimer();
        }
    }


    // Vignette
    [SerializeField] VignetteEffect _vignetteEffect;

    byte _timerCountdownKey;
    [SerializeField] float _darkenTime = 5;
    [SerializeField] UnityEvent OnKerasukan;
    IEnumerator TimerCountdown()
    {
        _vignetteEffect.gameObject.SetActive(true);
        byte requirement = ++_timerCountdownKey;
        // for(int i = 0; i < _darkenTime; i++)
        // {
        //     if(_timerCountdownKey != requirement) break;
        //     // _vignetteEffect.IncreaseLevel();
        //     yield return new WaitForSeconds(1);
        // }

        // if(_timerCountdownKey == requirement)
        // {
        //     OnKerasukan?.Invoke();
        // }

        float t = 4;
        while(t > 0 && _timerCountdownKey == requirement)
        {
            if(_isPaused)
            {
                yield return null;
                continue;
            }

            t -= Time.deltaTime;
            yield return null;

            if(t > 3 && t <= 4)
            {
                _vignetteEffect.SetLevel(0);
            }
            else if(t > 2 && t <= 3)
            {
                _vignetteEffect.SetLevel(1);
            }
            else if(t > 1 && t <= 2)
            {
                _vignetteEffect.SetLevel(2);
            }
            else if(t > 0 && t <= 1)
            {
                _vignetteEffect.SetLevel(3);
            }
        }
        if(_timerCountdownKey == requirement)
        {
            OnKerasukan?.Invoke();
        }
    }


    bool _isPaused = false;
    public void PauseTimer()
    {   
        _isPaused = true;
        _vignetteEffect.ResetLevel();
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }

    public void CancelTimer()
    {
        _timerCountdownKey++;
        _isPlayerInRadius = false;
        
        _vignetteEffect.ResetLevel();
        if(gameObject.activeInHierarchy) this.Invoke(_vignetteEffect.ResetLevel, 0.1f);
    }

}
