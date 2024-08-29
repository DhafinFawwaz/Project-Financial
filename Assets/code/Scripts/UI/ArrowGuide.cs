using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ArrowGuide : MonoBehaviour
{
    public static ArrowGuide Instance;
    Vector3 _targetPosition;
    
    [SerializeField] Vector3 _naoRikiPosition;
    [SerializeField] Vector3 _itbPosition;
    [SerializeField] Vector3 _supermarketPosition;
    [SerializeField] Vector3 _homePosition;

    [SerializeField] Vector3 _bedPosition;
    [SerializeField] Vector3 _pcPosition;
    [SerializeField] Vector3 _keluarHomePosition;
    void Start()
    {
        _mainCam = Camera.main;
        Refresh();
        Instance = this;
    }

    public void Refresh()
    {
        if(Save.Data.CurrentDay == 1 && !Save.Data.HasTalkedToNaoRikiInDay2)
        {
            _targetPosition = _naoRikiPosition;
            return;
        }   

        if(Save.Data.CurrentDayData.State == DayState.AfterSleeping) _targetPosition = _itbPosition;
        if(Save.Data.CurrentDayData.State == DayState.AfterBudgeting) _targetPosition = _supermarketPosition;
        if(Save.Data.CurrentDayData.State == DayState.AfterBelanja) _targetPosition = _homePosition;
        if(Save.Data.CurrentDayData.State == DayState.AfterStreaming) _targetPosition = _bedPosition;
    }

    Camera _mainCam;
    void Update()
    {
        Vector3 viewportPos = _mainCam.WorldToViewportPoint(_targetPosition);
        if(viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1)
        {
            transform.right = Vector3.down;
            transform.position = _mainCam.WorldToScreenPoint(_targetPosition);
        }
        else
        {
            Vector3 direction = _targetPosition - _mainCam.transform.position;
            direction = Vector3.ProjectOnPlane(direction, _mainCam.transform.forward);
            transform.right = direction;

            Vector3 screenPos = _mainCam.WorldToScreenPoint(_targetPosition);
            Vector3 edgePos = screenPos;
            if(viewportPos.x > 1) edgePos.x = _mainCam.pixelWidth;
            if(viewportPos.x < 0) edgePos.x = 0;
            if(viewportPos.y > 1) edgePos.y = _mainCam.pixelHeight;
            if(viewportPos.y < 0) edgePos.y = 0;

            transform.position = edgePos;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        float size = 0.1f;
        Gizmos.DrawCube(_naoRikiPosition,     new Vector3(size, 100, size));
        Gizmos.DrawCube(_itbPosition,         new Vector3(size, 100, size));
        Gizmos.DrawCube(_supermarketPosition, new Vector3(size, 100, size));
        Gizmos.DrawCube(_homePosition,        new Vector3(size, 100, size));
        Gizmos.DrawCube(_bedPosition,         new Vector3(size, 100, size));
        Gizmos.DrawCube(_pcPosition,          new Vector3(size, 100, size));
        Gizmos.DrawCube(_keluarHomePosition,  new Vector3(size, 100, size));
    }
}
