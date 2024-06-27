using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BudgetingManager : MonoBehaviour
{
    [SerializeField] PieChart _pieChart;
    [SerializeField] BelanjaList _belanjaList;
    [SerializeField] SceneTransition _sceneTransition;
    [SerializeField] string _sceneToLoad = "World";

    void Start()
    {
        _pieChart.SetAndAnimatePie(0.2f, 0.3f, 0.5f);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Save.Data.CurrentListBelanja  = _belanjaList.ListToBuy;
            Save.Data.CurrentBelanjaMoney = _belanjaList.CalculateTotalPrive();
            Save.SaveData();
            _sceneTransition.StartSceneTransition(_sceneToLoad);
        }
    }
}
