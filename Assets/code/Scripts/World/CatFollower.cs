using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatFollower : MonoBehaviour
{
    Transform _playerTrans;

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _skin;
    [SerializeField] SpriteMaterialAnimator _animator;
    Vector3 _initialScale;
    void Start()
    {
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
        _initialScale = _skin.localScale;
    }

    void FixedUpdate()
    {
        if(!_playerTrans) _playerTrans = PlayerCore.Instance.transform;

        if(!_agent.enabled) return;
        _agent.SetDestination(_playerTrans.position);
        _agent.transform.position = new Vector3(_agent.transform.position.x, 0, _agent.transform.position.z);

        if(_agent.velocity.x > 0) {
            _skin.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
        } else if(_agent.velocity.x < 0) {
            _skin.localScale = new Vector3(_initialScale.x, _initialScale.y, _initialScale.z);
        }

        _animator.SetDuration(1f/_agent.velocity.magnitude);
    }
}
