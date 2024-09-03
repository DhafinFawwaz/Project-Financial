using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClickToZoom : MonoBehaviour
{
    CinemachineVirtualCamera _vcam;
    [SerializeField] float _zoomFov = 6;
    float _initialFoV;

    void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();    
        _initialFoV = _vcam.m_Lens.FieldOfView;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine(TweenFov(_vcam, _vcam.m_Lens.FieldOfView, _zoomFov, 0.5f, Ease.OutQuart));
        } else if(Input.GetMouseButtonUp(1))
        {
            StartCoroutine(TweenFov(_vcam, _vcam.m_Lens.FieldOfView, _initialFoV, 0.5f, Ease.OutQuart));
        }
    }



    byte _key;
    IEnumerator TweenFov(CinemachineVirtualCamera vcam, float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _key)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            vcam.m_Lens.FieldOfView = Mathf.Lerp(start, end, easeFunction(t));
            yield return null;
        }
        if(requirement == _key)
        {
            vcam.m_Lens.FieldOfView = end;
        }
    }
}
