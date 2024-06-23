using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "OptionData", order = 1)]
public class OptionData : ScriptableObject
{
    public OptionContent[] Content => _content;
    [SerializeField] OptionContent[] _content;
}



[System.Serializable]
public class OptionContent
{
    [SerializeField] bool _diskon;
    [SerializeField] bool _inflasi;
    [SerializeField] bool _buy1get1;
    [SerializeField] int _price;
    [SerializeField] int _health;
    [SerializeField] int _happiness;
    [SerializeField] bool _isUp;
    public bool Diskon => _diskon;
    public bool Inflasi => _inflasi;
    public bool Buy1get1 => _buy1get1;
    public int Price => _price;
    public int Health => _health;
    public int Happiness => _happiness;
    public bool IsUp => _isUp;
}
