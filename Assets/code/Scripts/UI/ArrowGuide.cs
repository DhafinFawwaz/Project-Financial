using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ArrowGuide : MonoBehaviour
{
    public static ArrowGuide Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] float _jumpingSpeed = 1;
    [SerializeField] float _jumpingHeight = 1;
    Vector3 _targetPosition;
    public void Set(Vector3 pos)
    {
        _targetPosition = pos;
    }
    
    [SerializeField] Vector3 _naoRikiPosition;
    [SerializeField] Vector3 _itbPosition;
    [SerializeField] Vector3 _supermarketPosition;
    [SerializeField] Vector3 _homePosition;

    [SerializeField] Vector3 _bedPosition;
    [SerializeField] Vector3 _pcPosition;
    [SerializeField] Vector3 _keluarHomePosition;


#if UNITY_EDITOR
    [Header("Editor Only")]
    [SerializeField] Vector3 _testPosition;
    [ContextMenu("Test Position")]
    public void TestPosition()
    {
        if(_mainCam == null) _mainCam = Camera.main;
        _targetPosition = _testPosition;
        ApplyTargetPosition();
    }
#endif


    void Start()
    {
        _mainCam = Camera.main;
        Refresh();
    }

    public void Refresh()
    {
        Invoke(nameof(DelayedRefresh), 0.05f);
    }

    void DelayedRefresh()
    {
        if(Save.Data.CurrentDay == 1 && !Save.Data.HasTalkedToNaoRikiInDay2 && Save.Data.DayState == DayState.JustGotOutside)
        {
            _targetPosition = _naoRikiPosition;
            return;
        }   

        if(Save.Data.DayState == DayState.JustGotHome) _targetPosition = _pcPosition;
        if(Save.Data.DayState == DayState.AfterStreaming) _targetPosition = _bedPosition;
        if(Save.Data.DayState == DayState.AfterSleeping) _targetPosition = _keluarHomePosition;
        if(Save.Data.DayState == DayState.JustGotOutside) _targetPosition = _itbPosition;
        if(Save.Data.DayState == DayState.AfterKuliah) _targetPosition = _supermarketPosition;
        if(Save.Data.DayState == DayState.AfterBudgeting) _targetPosition = _supermarketPosition;
        if(Save.Data.DayState == DayState.AfterBelanja) _targetPosition = _homePosition;


        // Special case
        Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log(Save.Data.DayState);
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Bedroom" && (Save.Data.DayState == DayState.JustGotOutside || Save.Data.DayState == DayState.AfterKuliah || Save.Data.DayState == DayState.AfterBudgeting))
        {
            _targetPosition = _keluarHomePosition;
        }
    }

    Camera _mainCam;
    void ApplyTargetPosition()
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
    void Update()
    {
        ApplyTargetPosition();
        transform.position += Vector3.up * Parabole(Frac(Time.time * _jumpingSpeed)) * _jumpingHeight;
    }
    float Frac(float value)
    {
        return value - Mathf.Floor(value);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        float size = 0.025f;
        Gizmos.DrawCube(_naoRikiPosition,     new Vector3(size, 100, size));
        Gizmos.DrawCube(_itbPosition,         new Vector3(size, 100, size));
        Gizmos.DrawCube(_supermarketPosition, new Vector3(size, 100, size));
        Gizmos.DrawCube(_homePosition,        new Vector3(size, 100, size));
        Gizmos.DrawCube(_bedPosition,         new Vector3(size, 100, size));
        Gizmos.DrawCube(_pcPosition,          new Vector3(size, 100, size));
        Gizmos.DrawCube(_keluarHomePosition,  new Vector3(size, 100, size));
    }


    float Parabole(float x)
    {
        return -4 * (x - 0.5f) * (x - 0.5f) + 1;
    }
}
