using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeBelanja : MonoBehaviour
{
    [SerializeField] PopUp _popUp;
    [SerializeField] PopUp _popUpRil;
    [SerializeField] BelanjaManager _belanjaManager;
    void Start()
    {
        if(Save.Data.CurrentDay <= 1) {
            InputManager.SetActiveMouseAndKey(false);
            _popUp.Show();
        } else if(Save.Data.CurrentDay >= 2) {
            InputManager.SetActiveMouseAndKey(false);
            _popUpRil.Show();
        } else {
            _belanjaManager.gameObject.SetActive(true);
            _belanjaManager.StartGame();
        }
    }

    public void ClosePopUp()
    {
        _popUp.Hide();
        _belanjaManager.gameObject.SetActive(true);
        _belanjaManager.StartGame();
        InputManager.SetActiveMouseAndKey(true);
    }
    public void ClosePopUpRil()
    {
        _popUpRil.Hide();
        _belanjaManager.gameObject.SetActive(true);
        _belanjaManager.StartGame();
        InputManager.SetActiveMouseAndKey(true);
    }

    void Update()
    {
        if(InputManager.GetKeyDown(KeyCode.M))
        {
            InputManager.SetActiveMouseAndKey(true);
        }
    }
}
