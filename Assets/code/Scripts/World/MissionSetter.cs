using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionSetter : MonoBehaviour
{
    [SerializeField] Vector3 _arrowPosition;
    [SerializeField] string _missionText;
    [SerializeField] UnityEvent _onSet;

    public void Set()
    {
        ArrowGuide.Instance.Set(_arrowPosition);
        MissionText.Instance.Set(_missionText);
        _onSet?.Invoke();
    }


    [SerializeField] bool _showDebug = true;
    void OnDrawGizmos()
    {
        if(!_showDebug) return;
        Gizmos.color = UnityEngine.Color.yellow;
        float size = 0.025f;
        Gizmos.DrawCube(_arrowPosition,     new Vector3(size, 100, size));
    }
}
