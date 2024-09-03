using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToZoom : MonoBehaviour
{
    Camera _mainCam;
    [SerializeField] Transform _playerCamTarget;
    [SerializeField] float _zoomDuration = 0.5f;
    [SerializeField] float _zoomDistance = 5;
    [SerializeField] bool _canZoom = true;
    Vector3 _initialPosition;
    void Start()
    {
        _mainCam = Camera.main;
        _initialPosition = _playerCamTarget.position;
    }

    public void SetCanZoom(bool canZoom)
    {
        _canZoom = canZoom;
    }

    void Update()
    {
        if(!_canZoom) return;

        if(InputManager.GetMouseButton(0))
        {
            // Vector3 newPos = _mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zoomDistance));

            // Vector3 start = _playerCamTarget.position;

            // StartCoroutine(TweenPosition(_playerCamTarget, start, newPos, _zoomDuration, Ease.OutQuart));
            // _playerCamTarget.position = newPos;

            Vector3 mouseWorldPos = _mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zoomDistance));
            Vector3 distanceFromTarget = mouseWorldPos - _playerCamTarget.position;

            
        }
        else if(InputManager.GetMouseButtonUp(0))
        {
            Vector3 start = _playerCamTarget.position;
            StartCoroutine(TweenPosition(_playerCamTarget, start, _initialPosition, _zoomDuration, Ease.OutQuart));
        }
    }




    byte _posKey = 0;
    IEnumerator TweenPosition(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_posKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _posKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_posKey == requirement)
            rt.position = end;
    }
}
