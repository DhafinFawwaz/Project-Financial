using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBelanjaSetter : MonoBehaviour
{
    [SerializeField] WorldUI _worldUI;

    public void Set()
    {
        AfterBelanja.SetData(
            _worldUI.BelanjaList.ListCart,
            _worldUI.AddedHealth,
            _worldUI.AddedHealth
        );
    }
}
