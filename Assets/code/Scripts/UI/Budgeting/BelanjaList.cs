using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BelanjaList : MonoBehaviour
{
    [SerializeField] bool _clickTextToRemove = true;
    [SerializeField] TextMeshProUGUI _textPrefab;
    [SerializeField] Transform _contentParent;
    [SerializeField] int _maxAmount = 3;

    
    List<ItemCount> _listToBuy = new List<ItemCount>();
    List<ItemCount> _listCart = new List<ItemCount>();
    public List<ItemCount> ListToBuy => _listToBuy;
    public List<ItemCount> ListCart => _listCart;
    public UnityEvent<List<ItemCount>> OnListChange;

    public void AddItem(ItemData item)
    {
        if(_listToBuy.Exists(x => x.Item.Name == item.Name))
        {
            if(_listToBuy.Find(x => x.Item.Name == item.Name).Count >= _maxAmount) return;
            _listToBuy.Find(x => x.Item.Name == item.Name).Count++;
        }
        else
        {
            _listToBuy.Add(new ItemCount(item, 1));
        }
        UpdateDisplay();
    }

    public void RemoveItem(ItemData item)
    {
        if(_listToBuy.Exists(x => x.Item.Name == item.Name))
        {
            _listToBuy.Find(x => x.Item.Name == item.Name).Count--;
            if(_listToBuy.Find(x => x.Item.Name == item.Name).Count <= 0)
            {
                _listToBuy.Remove(_listToBuy.Find(x => x.Item.Name == item.Name));
            }
        }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        foreach(Transform child in _contentParent)
        {
            Destroy(child.gameObject);
        }
        foreach(ItemCount item in _listToBuy)
        {
            TextMeshProUGUI text = Instantiate(_textPrefab, _contentParent);
            text.text = item.Item.Name + " x" + item.Count;
            if(_clickTextToRemove)
            {
                Button button = text.GetComponent<Button>();
                button.onClick.AddListener(() => RemoveItem(item.Item));
            }

            // amount in cart
            int amountInCart = _listCart.Find(x => x.Item.Name == item.Item.Name)?.Count ?? 0;
            if(amountInCart == 0) continue;
            else if(amountInCart < item.Count) text.text += " (" + amountInCart + ")";
            else text.text = $"<s>{text.text} ({amountInCart})</s>";
        }
        OnListChange?.Invoke(_listToBuy);
    }

    public void SetList(List<ItemCount> list)
    {
        _listToBuy = list;
        UpdateDisplay();
    }

    public long CalculateTotalPrive()
    {
        long total = 0;
        foreach(ItemCount item in _listToBuy)
        {
            total += item.Item.Price * item.Count;
        }
        return total;
    }

    public void AddToCart(ItemData item)
    {
        if(_listCart.Exists(x => x.Item.Name == item.Name))
        {
            if(_listCart.Find(x => x.Item.Name == item.Name).Count >= _maxAmount) return;
            _listCart.Find(x => x.Item.Name == item.Name).Count++;
        }
        else
        {
            _listCart.Add(new ItemCount(item, 1));
        }
        UpdateDisplay();
    }
}

[System.Serializable]
public class ItemCount
{
    public ItemData Item;
    public int Count;
    public ItemCount(ItemData item, int count)
    {
        Item = item;
        Count = count;
    }
}