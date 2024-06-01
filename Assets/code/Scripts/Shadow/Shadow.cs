using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Shadow : MonoBehaviour
{
    [Header("References Outside")]
    [SerializeField] Transform _playerTrans;

    [Header("Properties")]
    [SerializeField] float _speed = 5;
    [SerializeField] Rigidbody _rb;
    

    [SerializeField] NavMeshAgent _agent;
    void Start()
    {
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
        StartCoroutine(TryMoveEverySecond());
    }

    void FixedUpdate()
    {
        _agent.SetDestination(_playerTrans.position);
    }

    public void StopMoving()
    {
        if(!_agent.isStopped) Debug.Log("Shadow is stopping");

        _agent.isStopped = true;
    }
    IEnumerator TryMoveEverySecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            _agent.isStopped = false;
        }
    }
}
