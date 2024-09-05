using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionText : MonoBehaviour
{
    public static MissionText Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] TextMeshProUGUI _text;

    public void Set(string text)
    {
        _text.text = text;
    }
}
