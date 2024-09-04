using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RakLockedObserver : MonoBehaviour
{
    [SerializeField] PopUp _okPopUp;
    GameObject okButton;
    bool _isPopUpActive = false;
    void Awake()
    {
        okButton = _okPopUp.transform.GetChild(0).gameObject;
        okButton.GetComponent<ButtonUI>().OnClick.AddListener(() => {
            EventSystem.current.SetSelectedGameObject(null);
            _isPopUpActive = false;
        });
    }

    void OnEnable()
    {
        Rak.s_OnAccessedWhenLocked += OnRakAccessedWhenLocked;
    }

    void OnDisable()
    {
        Rak.s_OnAccessedWhenLocked -= OnRakAccessedWhenLocked;
    }

    void OnRakAccessedWhenLocked()
    {
        if(_isPopUpActive) {
            InputManager.SetActiveAxisRawAndMouseButtonDown(true);
            _okPopUp.Hide();
            _isPopUpActive = false;
            EventSystem.current.SetSelectedGameObject(null);
        } else {
            InputManager.SetActiveAxisRawAndMouseButtonDown(false);
            _okPopUp.Show();
            EventSystem.current.SetSelectedGameObject(okButton);
            _isPopUpActive = true;
        }

    }

    public void OnOkPopUpClosed()
    {
        InputManager.SetActiveAxisRawAndMouseButtonDown(true);
    }

}
