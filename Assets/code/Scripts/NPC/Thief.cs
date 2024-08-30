using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Thief : MonoBehaviour
{
    Transform _playerTrans;

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _skin;
    [SerializeField] SpriteMaterialAnimator _animator;
    Vector3 _initialScale;
    
    [SerializeField] Transform _walletPrefab;
    [SerializeField] VignetteStolen _vignette;
    [SerializeField] WorldUI _worldUI;


    bool _hasPlayerStolen = false;

    bool SpawnCondition() {
        return true;
    }

    void OnStolen()
    {
        ApplyAnimation();
        _agent.speed = _runAwaySpeed;
        StartCoroutine(CollectAnimation(Instantiate(_walletPrefab, _playerTrans.position, Quaternion.identity), _playerTrans.position, _agent.transform, _walletJumpDuration, Vector3.zero, _walletJumpHeight));
        _vignette.Play();

        // Do Stuff
        Save.Data.NeedsMoney = 0;
        _worldUI.RefreshKTP();
    }

    void Start()
    {
        if(!SpawnCondition()) Destroy(gameObject);

		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
        _initialScale = _skin.localScale;
    }

    [SerializeField] Vector3 _areaCenter;
    [SerializeField] float _areaRadius = 20;
    [SerializeField] float _stoleRadius = 0.5f;
    [SerializeField] float _runAwaySpeed = 4;
    [SerializeField] float _distanceToHide = 20;
    [SerializeField] float _walletJumpHeight = 1;
    [SerializeField] float _walletJumpDuration = 0.5f;
    bool IsPlayerInArea()
    {
        if(!_playerTrans) return false;
        return Vector3.Distance(_playerTrans.position, _areaCenter) < _areaRadius;
    }

    bool IsPlayerInStoleArea()
    {
        if(!_playerTrans) return false;
        return Vector3.Distance(_playerTrans.position, _agent.transform.position) < _stoleRadius;
    }

#if UNITY_EDITOR
    [SerializeField] bool _drawDebug = true;
    void OnDrawGizmosSelected()
    {
        if(!_drawDebug) return;
        UnityEditor.Handles.color = new Color(1,0,0,0.1f);
        UnityEditor.Handles.DrawSolidDisc(_areaCenter, Vector3.up, _areaRadius);

        UnityEditor.Handles.color = new Color(0,1,0,0.1f);
        UnityEditor.Handles.DrawSolidDisc(_agent.transform.position, Vector3.up, _stoleRadius);
    }
#endif

    void FixedUpdate()
    {
        if(!_playerTrans) _playerTrans = PlayerCore.Instance.transform;
        if(!_agent.enabled) return;

        if(_hasPlayerStolen) {
            Vector3 runAwayDir = _agent.transform.position - _playerTrans.position;
            _agent.SetDestination(_agent.transform.position + runAwayDir.normalized);
            ApplyAnimation();
            HideIfFar();
            return;
        } else if(IsPlayerInStoleArea()) {
            _hasPlayerStolen = true;
            Vector3 runAwayDir = _agent.transform.position - _playerTrans.position;
            _agent.SetDestination(_agent.transform.position + runAwayDir.normalized);
            OnStolen();
            return;
        }

        if(!IsPlayerInArea()) {
            _agent.SetDestination(_areaCenter);
            return;
        }


        _agent.SetDestination(_playerTrans.position);
        ApplyAnimation();
    }
    void ApplyAnimation()
    {
        if(_agent.velocity.x > 0) {
            _skin.localScale = new Vector3(_initialScale.x, _initialScale.y, _initialScale.z);
        } else if(_agent.velocity.x < 0) {
            _skin.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
        }

        _animator.SetDuration(1f/_agent.velocity.magnitude);
    }

    void HideIfFar()
    {
        if(Vector3.Distance(_agent.transform.position, _playerTrans.position) > _distanceToHide) {
            Destroy(gameObject);
        }
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


}
