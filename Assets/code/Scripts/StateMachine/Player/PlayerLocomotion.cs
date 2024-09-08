using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerCore _core;
    [SerializeField] Rigidbody _rb;
    [SerializeField] bool _swapYAndZ = false;
    void Awake()
    {
        _core = GetComponent<PlayerCore>();
    }


    void FixedUpdate()
    {
        Move();
    }


    Vector3 _moveInputDirection;
    public void Move()
	{
		_moveInputDirection = _core.GetInputDirection();
        if(_swapYAndZ) _moveInputDirection = new Vector3(_moveInputDirection.x, _moveInputDirection.z, _moveInputDirection.y);
		
		float targetSpeed = _moveInputDirection.magnitude * _core.Stats.MoveSpeed;
        _moveInputDirection.x *= 0.75f;
		Vector3 speedDif = _moveInputDirection * _core.Stats.MoveSpeed - _rb.velocity;

		float accelerationRate;
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _core.Stats.Acceleration : _core.Stats.Deceleration;

		
        _rb.AddForce(accelerationRate * speedDif);
	}

}
