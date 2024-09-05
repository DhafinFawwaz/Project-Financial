using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerKerasukan : MonoBehaviour
{
    [SerializeField] ItemKerasukan[] _itemKerasukans;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _arriveDistance = 2f;

    [SerializeField]
    List<ItemKerasukan> _itemKerasukanCopy = new List<ItemKerasukan>();

    bool _isKerasukan = false;

    [SerializeField] GameObject _playerShadow;
    [SerializeField] GameObject _shadow;
    [SerializeField] GameObject _playerSkin;
    [SerializeField] Flashlight _flashLight;

    public void StartKerasukan()
    {
        _playerSkin.SetActive(false);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;
        _agent.Warp(_rb.position);
        _agent.enabled = true;

        CopyItemKerasukan();
        _isKerasukan = true;
        
        item = _itemKerasukanCopy[Random.Range(0, _itemKerasukanCopy.Count)];
        _itemKerasukanCopy.Remove(item);
        _agent.SetDestination(item.transform.position);
        
        _playerShadow.SetActive(true);
        _shadow.SetActive(false);
        _spacebarSpam.Play();
    }

    void CopyItemKerasukan()
    {
        foreach(ItemKerasukan item in _itemKerasukans)
        {
            _itemKerasukanCopy.Add(item);
        }
    }

    public void StopKerasukan()
    {
        _playerSkin.SetActive(true);
        _agent.enabled = false;
        _rb.useGravity = true;
        _rb.isKinematic = false;

        _itemKerasukanCopy.Clear();
        _isKerasukan = false;

        _playerShadow.SetActive(false);
        _shadow.SetActive(true);
        _shadow.transform.position = _agent.transform.position;
        _shadow.GetComponent<NavMeshAgent>().Warp(_agent.transform.position);

        PushShadowRandomDirection();
    }

    void PushShadowRandomDirection()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        HitResult _hitResult = new HitResult();
        HitRequest _hitRequest = _flashLight.HitRequest;
        _hitRequest.Direction = randomDirection;
        _shadow.GetComponent<Shadow>().OnHurt(_hitRequest, ref _hitResult);
    }


    ItemKerasukan item;
    [SerializeField] BelanjaList _belanjaList;
    [SerializeField] SpacebarSpam _spacebarSpam;
    void Update()
    {
        if(!_isKerasukan) return;

        if(Vector3.Distance(_agent.transform.position, item.transform.position) < _arriveDistance)
        {
            Buy(item);

            item = _itemKerasukanCopy[Random.Range(0, _itemKerasukanCopy.Count)];
            _itemKerasukanCopy.Remove(item);
            _agent.SetDestination(item.transform.position);
        }

        if(_itemKerasukanCopy.Count == 0)
        {
            CopyItemKerasukan();
        }
        _agent.transform.position = new Vector3(_agent.transform.position.x, 0, _agent.transform.position.z);
        


    }


    [SerializeField] GameObject _itemPrefab;
    [SerializeField] WorldUI _worldUI;
    void Buy(ItemKerasukan itemKerasukan)
    {
        // _belanjaList.AddToCart(itemKerasukan.ItemData);
        if(PlayerCore.Instance != null) 
        {
            GameObject item = Instantiate(_itemPrefab, _agent.transform.position, Quaternion.identity);
            item.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", itemKerasukan.ItemData.Sprite.texture);
            PlayerCore.Instance.Collect(item.transform);
            // _worldUI.AddItemFromOption(itemKerasukan.ItemData);
            _worldUI.AddItemFromAny(itemKerasukan.ItemData);
        }
    }

}
