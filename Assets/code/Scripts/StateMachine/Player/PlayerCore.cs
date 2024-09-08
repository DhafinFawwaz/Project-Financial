using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.GlobalIllumination;

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
    

    // I'm so tired, let's just Singleton this
    public static PlayerCore Instance = null;

    void Start()
    {
        States = new PlayerStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
        _camTargetInitialLocalPosition = _camTarget.localPosition;
        if(_vCam != null) _initialLensVerticalFOV = _vCam.m_Lens.FieldOfView;

        Instance = this;
    }

    public bool IsMoving()
    {
        return Locomotion.IsMoving();
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


    [Header("Collect Animation")]
    [SerializeField] float _collectDuration = 0.6f;
    [SerializeField] Vector3 _collectOffset = new Vector3(0,0.25f,0);
    [SerializeField] float _collectHeight = 0.8f;
    public void Collect(Transform item){
        StartCoroutine(CollectAnimation(item, item.position, transform, _collectDuration, _collectOffset, _collectHeight));
    }

    public void MoveCamera(Vector3 target){
        InputManager.SetActiveMouseAndKey(false);
        _locPosKey++;
        StartCoroutine(TweenPositionAnimation(_camTarget, _camTarget.position, target, 0.6f, Ease.OutQuart));
        StartCoroutine(TweenFOVAnimation(_vCam.m_Lens.FieldOfView, _zoomedLensVerticalFOV, 0.6f, Ease.OutQuart));
    }
    public void MoveCameraBack(){
        InputManager.SetActiveMouseAndKey(true);
        _posKey++;
        StartCoroutine(TweenLocalPositionAnimation(_camTarget, _camTarget.localPosition, _camTargetInitialLocalPosition, 0.6f, Ease.OutQuart));
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
