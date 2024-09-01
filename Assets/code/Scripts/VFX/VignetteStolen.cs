using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VignetteStolen : MonoBehaviour
{
    [SerializeField] float _fovStart = 13;
    [SerializeField] float _fovEnd = 5;
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] GraphicsAnimation _vignette;

    [SerializeField] float _timeScale = 0.5f;

    public void Play()
    {
        StartCoroutine(TweenFov(_cam, _fovStart, _fovEnd, 0.5f, Ease.OutQuart));
        _vignette.gameObject.SetActive(true);
        _vignette.SetEndColor(new Color(0,0,0,0.5F));
        _vignette.Play();
        Time.timeScale = _timeScale;
        this.Invoke(() => {
            StartCoroutine(TweenFov(_cam, _fovEnd, _fovStart, 0.5f, Ease.InQuart));
            _vignette.SetEndColor(new Color(0,0,0,0));
            _vignette.Play();
            Time.timeScale = 1;
        }, 0.5f);
    }

    byte _key = 0;
    IEnumerator TweenFov(CinemachineVirtualCamera cg, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _key == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            cg.m_Lens.FieldOfView = Mathf.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_key == requirement)
        {
            cg.m_Lens.FieldOfView = end;
        }
    }


}
