using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
