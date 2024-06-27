using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CatalogGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _buttonTypes;
    // 0: happiness
    // 1: health
    // 2: both
    
    [SerializeField] ItemData[] _makanan;
    [SerializeField] ItemData[] _minuman;
    [SerializeField] ItemData[] _cemilan;
    [SerializeField] ItemData[] _rumah;
    [SerializeField] ItemData[] _random;

    [SerializeField] Transform _makananParent;
    [SerializeField] Transform _minumanParent;
    [SerializeField] Transform _cemilanParent;
    [SerializeField] Transform _rumahParent;
    [SerializeField] Transform _randomParent;
    [SerializeField] UnityEvent<ItemData> OnItemClicked;

    void Start()
    {
        Spawn(PickRandomUnique(_makanan, Mathf.CeilToInt(_makanan.Length/2)), _makananParent);
        Spawn(PickRandomUnique(_minuman, Mathf.CeilToInt(_minuman.Length/2)), _minumanParent);
        Spawn(PickRandomUnique(_cemilan, Mathf.CeilToInt(_cemilan.Length/2)), _cemilanParent);
        Spawn(PickRandomUnique(_rumah, Mathf.CeilToInt(_rumah.Length/2)), _rumahParent);
        Spawn(PickRandomUnique(_random, Mathf.CeilToInt(_random.Length/2)), _randomParent);
    }

    ItemData[] PickRandomUnique(ItemData[] items, int amount)
    {
        if(amount > items.Length) return items;
        ItemData[] result = new ItemData[amount];
        List<ItemData> temp = new List<ItemData>(items);
        for(int i = 0; i < amount; i++)
        {
            int index = UnityEngine.Random.Range(0, temp.Count);
            result[i] = temp[index];
            temp.RemoveAt(index);
        }
        return result;
    }

    void Spawn(ItemData[] items, Transform parent)
    {
        foreach(ItemData item in items)
        {
            GameObject obj = null;
            if(item.Health > 0 && item.Happiness > 0)
            {
                obj = Instantiate(_buttonTypes[2], parent);
            }
            else if(item.Health > 0)
            {
                obj = Instantiate(_buttonTypes[1], parent);
            }
            else if(item.Happiness > 0)
            {
                obj = Instantiate(_buttonTypes[0], parent);
            }
            else Debug.LogError("Item " + item.Name + " has no health nor happiness value");

            obj.GetComponent<ButtonUI>().OnClick.AddListener(() => {
                OnItemClicked?.Invoke(item);
            });
            obj.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = item.Sprite;
            obj.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = item.Price.ToStringRupiahFormat();
        }
    }
}
