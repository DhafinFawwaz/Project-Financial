using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateTarget : MonoBehaviour
{
    [SerializeField] int _targetFramerate = -1;
    void Awake()
    {
        Application.targetFrameRate = _targetFramerate;
    }
    public void SetTargetFramerate()
    {
        Application.targetFrameRate = _targetFramerate;
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(FramerateTarget))]
public class FramerateTargetEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FramerateTarget target = this.target as FramerateTarget;
        if(GUILayout.Button("Set Target Framerate"))
        {
            target.SetTargetFramerate();
        }
    }
}
#endif