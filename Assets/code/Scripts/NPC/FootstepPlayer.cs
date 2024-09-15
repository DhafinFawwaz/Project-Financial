using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    Vector3 _velocity;
    Vector3 _lastPos;


    float _lastFootStepTime = 0;
    [SerializeField] float _footstepCooldown = 0.5f;
    float _minVelocity = 0.1f;
    public static System.Action s_OnFootstep;
    void CheckFootsteps()
    {
        if(Time.time - _lastFootStepTime < _footstepCooldown) return;
        
        if(_velocity.x > _minVelocity || _velocity.z > _minVelocity || _velocity.x < -_minVelocity || _velocity.z < -_minVelocity)
        {
            _lastFootStepTime = Time.time;
            s_OnFootstep?.Invoke();
        }
    }


    void Update()
    {
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
        CheckFootsteps();
    }
}
