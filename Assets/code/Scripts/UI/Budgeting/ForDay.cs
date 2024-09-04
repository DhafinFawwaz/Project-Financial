using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForDay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _dayText;

    void Awake()
    {
        _dayText.text = "Untuk Hari ke- " + (Save.Data.CurrentDay+1).ToString() + ", " + (Save.Data.CurrentDay+2).ToString() + ", " + (Save.Data.CurrentDay+3).ToString() + ", ";
    }
}
