using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Toaster : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textTitle;
    [SerializeField] TextMeshProUGUI _textMessage;

    public void SetText(string title, string message)
    {
        _textTitle.text = title;
        _textMessage.text = message;
    }
}
