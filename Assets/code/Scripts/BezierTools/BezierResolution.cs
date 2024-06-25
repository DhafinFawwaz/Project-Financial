using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BezierResolution : MonoBehaviour
{
    [SerializeField] PathCreator _pathCreator;
    [Range(0, 10)]
    [SerializeField] int _subDivision = 1; // amount of points to add
    public void ApplyResolution()
    {
        Vector2 start = _pathCreator.bezierPath.GetPoint(0);
        Vector3 end = _pathCreator.bezierPath.GetPoint(_pathCreator.bezierPath.NumPoints-1);

        float t = 1 / (float)_subDivision;
        Vector3 firstPoint = Vector3.Lerp(start, end, t);
        _pathCreator.bezierPath.SetPoint(1, firstPoint, true);

        // for (int i = 2; i < _subDivision; i++)
        // {
        //     t = i / (float)_subDivision;
        //     Vector3 point = Vector3.Lerp(start, end, t);
        //     _pathCreator.bezierPath.AddSegmentToEnd(point);
        //     _pathCreator.bezierPath.SetPoint(i, point, true);
        // }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BezierResolution))]
public class BezierResolutionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BezierResolution br = (BezierResolution)target;
        if(GUILayout.Button("Apply Resolution"))
        {
            br.ApplyResolution();
        }
    }
}
#endif