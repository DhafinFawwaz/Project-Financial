using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class AfterBelanja : MonoBehaviour
{
    static List<ItemCount> _listCart = new List<ItemCount>();
    static float _addedHealth;
    static float _addedhappiness;

    [SerializeField] TextMeshProUGUI _barangText;
    [SerializeField] TextMeshProUGUI _hargaText;
    [SerializeField] TextMeshProUGUI _totalhargaText;
    [SerializeField] TextMeshProUGUI _sisaText;
    [SerializeField] TextMeshProUGUI _addedHealthText;
    [SerializeField] TextMeshProUGUI _addedHapinesssisaText;

    [SerializeField] KTPWorld _ktpWorld;

    public static void SetData(
        List<ItemCount> listCart,
        float addedHealth,
        float addedhappiness
    )
    {
        _listCart = listCart;
        _addedHealth = addedHealth;
        _addedhappiness = addedhappiness;
    }

    void Awake()
    {
        _ktpWorld.SetMoney(Save.Data.Money)
            .SetHappiness(Save.Data.Happiness)
            .SetHealth(Save.Data.Health);

        _barangText.text = "";
        _hargaText.text = "";
        

        foreach(var item in _listCart)
        {
            _barangText.text += $"{item.Item.Name} x{item.Count}\n";
            _hargaText.text += $"{item.Item.Price*item.Count}\n";
        }

        float totalHarga = 0;
        foreach(var item in _listCart)
        {
            totalHarga += item.Item.Price * item.Count;
        }

        _totalhargaText.text = totalHarga.ToString();
        _sisaText.text = (Save.Data.Money - totalHarga).ToString();
        _addedHealthText.text = _addedHealth.ToString();
        _addedHapinesssisaText.text = _addedhappiness.ToString();
        
    }


    [SerializeField] UnityEvent onE;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            onE?.Invoke();
        }
    }
}
