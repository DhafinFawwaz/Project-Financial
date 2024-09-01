using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;

public class EndStreamButton : MonoBehaviour
{
    [SerializeField] GameObject _button;
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if(Save.Data.CurrentDayData.StreamingCounter == 0) _button.SetActive(false);
        else _button.SetActive(true);
    }

    [SerializeField] UnityEvent _onEDown;
    void Update()
    {
        if(Save.Data.CurrentDayData.StreamingCounter == 0) return;
        if(InputManager.GetKeyDown(KeyCode.E)) {
            _onEDown?.Invoke();
            enabled = false;
        }
    }
}
