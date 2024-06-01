using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static Action<PlayerCore> s_OnPlayerMoneyUpdated;

    PlayerCore _core;
    public float MoveSpeed = 10;
    public float Acceleration = 10;
    public float Deceleration = 2;

    long _money = 100;
    public long Money{
        get => _money;
        set {
            _money = value;
            s_OnPlayerMoneyUpdated?.Invoke(_core);
        }
    
    }


    void Awake()
    {
        _core = GetComponent<PlayerCore>();
    }

    
}
