using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerCore : Core<PlayerCore, PlayerStates>
{
    public PlayerLocomotion Locomotion;
    public PlayerStats Stats;

    // public IInteractable CurrentInteractableObject;
    [SerializeField] Transform _camTarget;
    Vector3 _camTargetInitialLocalPosition;

    [Header("Camera")]
    [SerializeField] float _zoomedLensVerticalFOV = 6;
    [SerializeField] CinemachineVirtualCamera _vCam;
    float _initialLensVerticalFOV;
    
    static Vector3 _spawnPosition = Vector3.zero;
    static bool _movePlayerToSpawnPoint = true;
    public static void SetPlayerSpawnPosition(Vector3 position){
        _spawnPosition = position;
        _movePlayerToSpawnPoint = true;
    }

    void Start()
    {
        States = new PlayerStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
        _camTargetInitialLocalPosition = _camTarget.localPosition;
        if(_vCam != null) _initialLensVerticalFOV = _vCam.m_Lens.FieldOfView;


        if(_movePlayerToSpawnPoint){
            transform.position = _spawnPosition;
            _movePlayerToSpawnPoint = false;
        }
    }

    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {

    }


    void Update()
    {
        CurrentState.StateUpdate();
    }
    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    bool CanBuy()
    {
        // if(!CurrentInteractableObject.CanInteract()) return;
        return true;
    }

    public Vector3 GetInputDirection(){
        return new Vector3(InputManager.GetAxisRaw("Horizontal"), 0, InputManager.GetAxisRaw("Vertical"));
    }



    public void Collect(Item item){
        StartCoroutine(CollectAnimation(item.transform, item.transform.position, transform, 0.6f, new Vector3(0,0.25f,0), 1));
    }

    public void MoveCamera(Vector3 target){
        InputManager.SetActiveMouseAndKey(false);
        StartCoroutine(TweenPositionAnimation(_camTarget, _camTarget.position, target, 0.6f, Ease.OutQuart));
        StartCoroutine(TweenFOVAnimation(_vCam.m_Lens.FieldOfView, _zoomedLensVerticalFOV, 0.6f, Ease.OutQuart));
    }
    public void MoveCameraBack(){
        InputManager.SetActiveMouseAndKey(true);
        StartCoroutine(TweenPositionAnimation(_camTarget, _camTarget.localPosition, _camTargetInitialLocalPosition, 0.6f, Ease.OutQuart));
        StartCoroutine(TweenFOVAnimation(_vCam.m_Lens.FieldOfView, _initialLensVerticalFOV, 0.6f, Ease.OutQuart));
    }

    IEnumerator CollectAnimation(Transform rt, Vector3 start, Transform endTrans, float duration, Vector3 offset, float height)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, endTrans.position, t) + offset + Vector3.up * height * Parabole(t);
            yield return null;
        }
        rt.position = endTrans.position;
        Destroy(rt.gameObject);
    }


    float Parabole(float x) => -4 * x * x + 4 * x;


    
    byte _posKey = 0;
    IEnumerator TweenPositionAnimation(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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

    byte _locPosKey;
    IEnumerator TweenLocalPositionAnimation(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_locPosKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _locPosKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_locPosKey == requirement)
            rt.localPosition = end;
    }

    byte _fovKey;
    IEnumerator TweenFOVAnimation(float start, float end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_fovKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _fovKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            _vCam.m_Lens.FieldOfView = Mathf.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_fovKey == requirement)
            _vCam.m_Lens.FieldOfView = end;
    }
}
