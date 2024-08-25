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
            WorldUI.AddedHealth,
            WorldUI.AddedHappiness
        );
    }
}
