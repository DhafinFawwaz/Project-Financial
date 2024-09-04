using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Seller : Interactable
{
    [Serializable]
    public class Snack
    {
        public string Name;
        public int Index;
        public int Happiness;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public int Stock;
        public int Price;
    }

    public static Action<Seller> s_OnNPCInteract;

    [SerializeField] Snack _snack;
    public Snack SoldSnack => _snack;

    [SerializeField] TextMeshPro _itemName;
    [SerializeField] TextMeshPro _itemStock;
    [SerializeField] TextMeshPro _itemPrice;
    [SerializeField] TextMeshPro _itemHapiness;

    [SerializeField] Transform _itemPrefab;
    
    
    
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        _snack.Stock = Save.Data.HapinessItemStocks[_snack.Index];
        _itemName.text = _snack.Name;
        _itemStock.text = _snack.Stock.ToString();
        _itemPrice.text = _snack.Price.ToString();
        _itemHapiness.text = _snack.Happiness.ToString();
    }


    protected override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        s_OnNPCInteract?.Invoke(this);
        Refresh();
    }


    [SerializeField] float _throwItemScale = 0.22f;
    public void ThrowItem()
    {
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
        item.localScale = Vector3.one * _throwItemScale;
        PlayerCore.Instance.Collect(item);
    }
}
