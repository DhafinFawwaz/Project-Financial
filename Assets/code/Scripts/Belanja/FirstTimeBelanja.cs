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
        if(Save.Data.CurrentDay == 1) {
            _popUp.Show();
        } else if(Save.Data.CurrentDay >= 2) {
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
    }
    public void ClosePopUpRil()
    {
        _popUpRil.Hide();
        _belanjaManager.gameObject.SetActive(true);
        _belanjaManager.StartGame();
    }
}
