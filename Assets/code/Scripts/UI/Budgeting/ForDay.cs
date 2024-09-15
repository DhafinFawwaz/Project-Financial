using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForDay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _day1Text;
    [SerializeField] TextMeshProUGUI _day2Text;
    [SerializeField] TextMeshProUGUI _day3Text;

    void Awake()
    {
        _day1Text.text = (Save.Data.CurrentDay+1).ToString();
        _day2Text.text = (Save.Data.CurrentDay+2).ToString();
        _day3Text.text = (Save.Data.CurrentDay+3).ToString();
    }
}
