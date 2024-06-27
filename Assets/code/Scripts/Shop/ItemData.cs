using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] long _price;
    [SerializeField] double _health;
    [SerializeField] double _happiness;
    [SerializeField] Sprite _sprite;
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public long Price => _price;
    public double Health => _health;
    public double Happiness => _happiness;
    
}