using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAfterDayState : MonoBehaviour
{
    [SerializeField] DialogData[] _dialogJustGotHome;
    [SerializeField] DialogData[] _dialogAfterKuliah;
    [SerializeField] DialogData[] _dialogAfterBelanja;
    [SerializeField] DialogData[] _dialogAfterBudgeting;
    void Start()
    {
        if(Save.Data.DayState == DayState.AfterKuliah && _dialogAfterKuliah[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterKuliah[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterBelanja && _dialogAfterBelanja[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterBelanja[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterBudgeting && _dialogAfterBudgeting[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterBudgeting[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.JustGotHome && _dialogJustGotHome[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogJustGotHome[Save.Data.CurrentDay]);
    }
}
