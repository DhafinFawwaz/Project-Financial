using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected Transform _interactablePromptAnchor;
    [SerializeField] UnityEvent _onInteract;

    string _playerTag = "Player";
    bool _isShowingPrompt = false;
    public bool IsShowingPrompt => _isShowingPrompt;
    protected PlayerCore _playerInstance = null;

    void Update()
    {
        if(_isShowingPrompt && InputManager.GetKeyDown(KeyCode.E))
        {
            OnPlayerInteract();
        }
    }

    protected virtual void OnPlayerInteract()
    {
        _onInteract?.Invoke();
    }

    public void OnTriggerEnter(Collider col)
    {
        if(_playerInstance == null) _playerInstance = col.GetComponent<PlayerCore>();
        if(col.CompareTag(_playerTag)) SetActiveLabel(true);
    }
    public void OnTriggerExit(Collider col)
    {
        if(col.CompareTag(_playerTag)) SetActiveLabel(false);
    }

    public void SetActiveLabel(bool isActive)
    {
        _isShowingPrompt = isActive;
        if(_isShowingPrompt){
            _interactablePromptAnchor.gameObject.SetActive(true);
            StartCoroutine(TweenLocalScale(_interactablePromptAnchor.transform, _interactablePromptAnchor.transform.localScale, Vector3.one, 0.15f, Ease.OutBackQuart));
        }
        else {
            StartCoroutine(TweenLocalScale(_interactablePromptAnchor.transform, _interactablePromptAnchor.transform.localScale, Vector3.zero, 0.15f, Ease.InCubic, () => _interactablePromptAnchor.gameObject.SetActive(false)));
        }
    }


    byte _scaleKey = 0;
    protected IEnumerator TweenLocalScale(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction, Action onComplete = null)
    {
        byte requirement = ++_scaleKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _scaleKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_scaleKey == requirement)
        {
            rt.localScale = end;
            onComplete?.Invoke();
        }
    }
}
