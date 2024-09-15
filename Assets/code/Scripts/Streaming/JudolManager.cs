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
            this.Invoke(_judolPopUp.Show, 1f);
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
