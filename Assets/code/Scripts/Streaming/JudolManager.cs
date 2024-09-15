using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudolManager : MonoBehaviour
{
    [SerializeField] PopUp _judolPopUp;
    [SerializeField] PopUp _okPopUp;
    [SerializeField] StreamCutscene _streamCutscene;

    void Start()
    {
        if(Save.Data.CurrentDay == 13) {
            _streamCutscene.enabled = false;
            _judolPopUp.Show();
        }
        else Destroy(_judolPopUp.transform.parent.gameObject);
    }

    public void CloseAll()
    {
        _judolPopUp.Hide();
        _okPopUp.Hide();
        _streamCutscene.enabled = true;
    }
}
