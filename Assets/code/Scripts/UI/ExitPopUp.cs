using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopUp : MonoBehaviour
{
    [SerializeField] PopUp _exitPopUp;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _exitPopUp.Show();
        }
    }

}
