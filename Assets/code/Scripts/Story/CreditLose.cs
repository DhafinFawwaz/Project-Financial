using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreditLose : MonoBehaviour
{
    void Awake()
    {
        if(Save.Data.IsPinjol) LoadPinjol();
    }
    void LoadPinjol()
    {
        _sceneTransition.StartSceneTransition("PinjolLose");
    }

    [SerializeField] SceneTransition _sceneTransition;
    public void LoseCheck()
    {
        if(WillDefinitelyLose()) {
            PopUpLose();
        }
    }

    void PopUpLose()
    {
    }

    public void ChooseNo()
    {
        _sceneTransition.StartSceneTransition("CreditLose");
        Save.Data.IsLose = true;
    }

    void ChoosePinjol()
    {
        Save.Data.NeedsMoney += 1000000000;
        Save.Data.DesireMoney += 1000000000;
        Save.Data.DebitMoney += 1000000000;
        Save.Data.DebitTabunganMoney += 1000000000;
        Save.Data.IsPinjol = true;
    }

    bool WillDefinitelyLose()
    {
        // TODO: Test this function
        // check if the unrealized money is less than the debt with deadline of 1 day left

        long salaryToGain = Save.Data.GetSalaryFromToday();
        long debtWithNearDeadline = 0;
        for(int i = 0; i < Save.Data.DayDatas.Count; i++)
        {
            if(Save.Data.DayDatas[i].CreditMoney != 0 && Save.Data.CurrentDay - i == 3)
            {
                debtWithNearDeadline = Save.Data.DayDatas[i].CreditMoney;
                break;
            }
        }

        long totalMoney = Save.Data.DebitMoney + Save.Data.DebitTabunganMoney + Save.Data.NeedsMoney + Save.Data.DesireMoney + salaryToGain;
        if(totalMoney < debtWithNearDeadline) return true;
        return false;
    }
}
