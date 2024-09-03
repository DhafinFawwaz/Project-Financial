using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "OptionData", order = 1)]
public class OptionData : ScriptableObject
{
    [SerializeField] ItemData _itemData;
    public ItemData ItemData => _itemData;
    public string ItemName => _itemData.Name;
    public Sprite ItemSprite => _itemData.Sprite;
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
    [SerializeField] int _quality;
    public bool Diskon {get => _diskon; set => _diskon = value;}
    public bool Inflasi {get => _inflasi; set =>_inflasi = value;}
    public bool Buy1get1 {get => _buy1get1; set => _buy1get1 = value;}
    public int Price {get => _price; set => _price = value;}
    public int Health {get => _health; set => _health = value;}
    public int Happiness {get => _happiness; set => _happiness = value;}
    public bool IsUp {get => _isUp; set => _isUp = value;}
    public int Quality {get => _quality; set => _quality = value;}
}
