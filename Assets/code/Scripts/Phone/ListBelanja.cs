using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListBelanja : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _text;
    
    [System.Serializable]
    class ItemCount
    {
        public Item Item;
        public int Count;
        public ItemCount(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }
    [SerializeField] List<ItemCount> list = new List<ItemCount>();

    List<ItemCount> inventory = new List<ItemCount>();
    void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for(int i = 0; i < _text.Length; i++)
        {
            if(i >= list.Count) {
                _text[i].text = "";
                continue;
            }
            _text[i].text = "";
            if(inventory.Find(x => x.Item.ItemName == list[i].Item.ItemName)?.Count >= list[i].Count){
                _text[i].text = "<s>" + list[i].Item.ItemName + " x" + list[i].Count + "</s>";
            } else {
                _text[i].text = list[i].Item.ItemName + " x" + list[i].Count;
            }
        }
    }


    public void AddInventory(Item item, int count)
    {
        if(inventory.Exists(x => x.Item == item))
        {
            inventory.Find(x => x.Item.ItemName == item.ItemName).Count++;
        }
        else
        {
            inventory.Add(new ItemCount(item, count));
        }
    }
}
