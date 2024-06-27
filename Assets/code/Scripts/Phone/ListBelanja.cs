using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ListBelanja : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _text;
    
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
    [SerializeField] List<ItemCount> list = new List<ItemCount>();

    List<ItemCount> inventory = new List<ItemCount>();

    public UnityEvent<List<ItemCount>> OnListChange;
    void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if(_text == null || _text.Length == 0) return;
        
        for(int i = 0; i < _text.Length; i++)
        {
            if(i >= list.Count) {
                _text[i].text = "";
                continue;
            }
            _text[i].text = "";
            if(inventory.Find(x => x.Item.Name == list[i].Item.Name)?.Count >= list[i].Count){
                _text[i].text = "<s>" + list[i].Item.Name + " x" + list[i].Count + "</s>";
            } else {
                _text[i].text = list[i].Item.Name + " x" + list[i].Count;
            }
        }
        OnListChange?.Invoke(list);
    }


    public void AddInventory(ItemData item, int count)
    {
        if(inventory.Exists(x => x.Item == item))
        {
            inventory.Find(x => x.Item.Name == item.Name).Count++;
        }
        else
        {
            inventory.Add(new ItemCount(item, count));
        }
    }
}
