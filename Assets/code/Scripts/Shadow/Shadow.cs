using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Shadow : MonoBehaviour
{
    [Header("References Outside")]
    [SerializeField] Transform _playerTrans;

    [Header("Properties")]
    [SerializeField] float _speed = 5;
    [SerializeField] Rigidbody _rb;
    

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] EnemySkin _skin;
    [SerializeField] float _stopTimeDuration = 0.1f;

    [SerializeField] ShadowNearAttack _shadowNearAttack;
    [SerializeField] UnityEvent _onHurt;
    void Start()
    {
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
    }

    void FixedUpdate()
    {
        if(!_agent.enabled) return;
        _agent.SetDestination(_playerTrans.position);
        _agent.transform.position = new Vector3(_agent.transform.position.x, 0, _agent.transform.position.z);
    }


    public void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        if(!_agent.enabled) return;
        hitResult.Type = HitType.Entity;
        _skin.PlayHurtAnimation();
        StartCoroutine(ApplyKnockback(_rb.position + hitRequest.Direction * hitRequest.Knockback, hitRequest.StunDuration));
        _onHurt?.Invoke();
    }

    private IEnumerator ApplyKnockback(Vector3 force, float _stoppedTime = 3f)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_stopTimeDuration);
        Time.timeScale = 1f;

        yield return null;
        _agent.enabled = false;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.AddForce(force);

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(_stoppedTime);

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _agent.Warp(transform.position);
        _agent.enabled = true;

    }


    public void Freeze()
    {
        _shadowNearAttack.CancelTimer();
        _shadowNearAttack.enabled = false;
        _agent.enabled = false;
        enabled = false;
    }

    public void UnFreeze()
    {
        _shadowNearAttack.enabled = true;
        _agent.enabled = true;
        enabled = true;
    }
}
