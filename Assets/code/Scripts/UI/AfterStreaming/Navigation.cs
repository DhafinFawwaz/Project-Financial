using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] ButtonUI[] _buttons;
    [SerializeField] GameObject[] _menus;
    [SerializeField] int _firstActiveMenu = 0;
    void Awake()
    {
        for(int i = 0; i < _buttons.Length; i++) {
            int idx = i;
            _buttons[i].OnClick.AddListener(() => {
                Debug.Log("idx: " + idx);
                for(int j = 0; j < _menus.Length; j++) {
                    _menus[j].SetActive(j == idx);
                    _buttons[j].SetInteractable(j != idx);
                }
            });
        }

        // first active
        _buttons[_firstActiveMenu].OnClick?.Invoke();
    }
}
