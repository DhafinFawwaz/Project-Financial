using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "DayData", menuName = "DayData", order = 1)]
public class DayData : ScriptableObject
{
    [System.Serializable]
    public class DayState
    {
        public List<int> HealthItemStocks = new List<int>();
        public List<int> HapinessItemStocks = new List<int>();
    }
    public DayState[] State => _state;
    [SerializeField] DayState[] _state;
}

#if UNITY_EDITOR
[CustomEditor(typeof(DayData))]
public class DayDataEditor : Editor
{
    void Randomize(DayData dayData)
    {
        for (int i = 0; i < dayData.State.Length; i++)
        {
            for (int j = 0; j < dayData.State[i].HealthItemStocks.Count; j++)
            {
                dayData.State[i].HealthItemStocks[j] = UnityEngine.Random.Range(1, 4);
            }
            for (int j = 0; j < dayData.State[i].HapinessItemStocks.Count; j++)
            {
                dayData.State[i].HapinessItemStocks[j] = UnityEngine.Random.Range(1, 4);
            }
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DayData dayData = (DayData)target;
        if (GUILayout.Button("Randomize"))
        {
            Randomize(dayData);
        }
        
    }
}

#endif