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

    public void SetButtonActive(bool active)
    {
        _button.SetActive(active);
    }

    [SerializeField] UnityEvent _onEDown;
    void Update()
    {
        if(Save.Data.CurrentDayData.StreamingCounter == 0) return;
        if(InputManager.GetKeyDown(KeyCode.E) && _button.activeInHierarchy) {
            _onEDown?.Invoke();
            enabled = false;
        }
    }
}
