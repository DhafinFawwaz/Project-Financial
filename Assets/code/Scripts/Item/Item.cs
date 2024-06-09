using UnityEngine;
using System.Collections;
using TMPro;

public class Item : MonoBehaviour
{
    string _playerTag = "Player";
    bool _isCollected = false;
    Vector3 _targetRotation = new Vector3(0,720,0);
    Vector3 _targetPositionOffset = new Vector3(0,1,0);
    [SerializeField] TextMeshPro _label;
    [SerializeField] Transform _labelAnchor;
    [SerializeField] string _itemName;
    public string ItemName => _itemName;
    bool _isShowingLabel = false;
    void Start() {
        _label.text = _itemName;
    }
    public void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(_playerTag)) ToggleLabel();
    }
    public void OnTriggerExit(Collider col)
    {
        if(col.CompareTag(_playerTag)) ToggleLabel();
    }

    void ToggleLabel()
    {
        if(_isCollected) return;
        _isShowingLabel = !_isShowingLabel;
        if(_isShowingLabel)
            StartCoroutine(TweenLocalScale(_labelAnchor.transform, _labelAnchor.transform.localScale, Vector3.one, 0.3f, Ease.OutQuart));
        else
            StartCoroutine(TweenLocalScale(_labelAnchor.transform, _labelAnchor.transform.localScale, Vector3.zero, 0.3f, Ease.OutQuart));
    }

    void Collect(PlayerCore player) {
        _isCollected = true;
        player.Collect(this);

        // Collected animation
        StartCoroutine(TweenEulerAngles(transform, transform.localEulerAngles, _targetRotation, 0.6f, Ease.OutQuart));
        StartCoroutine(TweenPosition(transform, transform.position, transform.position+_targetPositionOffset, 0.6f, Ease.OutQuart));
        StartCoroutine(TweenLocalScale(transform, transform.localScale, Vector3.zero, 0.4f, Ease.InQuart));
    }



    byte _rotKey = 0;
    IEnumerator TweenEulerAngles(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_rotKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && _rotKey == requirement)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.localEulerAngles = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(_rotKey == requirement)
            rt.localEulerAngles = end;
    }


    byte _posKey = 0;
    IEnumerator TweenPosition(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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


    byte _scaleKey = 0;
    IEnumerator TweenLocalScale(Transform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
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
            rt.localScale = end;
    }
}