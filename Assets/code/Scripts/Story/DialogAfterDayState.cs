using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAfterDayState : MonoBehaviour
{
    [SerializeField] DialogData[] _dialogJustGotHome;
    [SerializeField] DialogData[] _dialogAfterStreaming;
    [SerializeField] DialogData[] _dialogAfterSleeping;
    [SerializeField] DialogData[] _dialogAfterKuliah;
    [SerializeField] DialogData[] _dialogAfterBelanja;
    [SerializeField] DialogData[] _dialogAfterBudgeting;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if(Save.Data.DayState == DayState.AfterKuliah && _dialogAfterKuliah[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterKuliah[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterBelanja && _dialogAfterBelanja[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterBelanja[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterBudgeting && _dialogAfterBudgeting[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterBudgeting[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.JustGotHome && _dialogJustGotHome[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogJustGotHome[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterStreaming && _dialogAfterStreaming[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterStreaming[Save.Data.CurrentDay]);
        else if(Save.Data.DayState == DayState.AfterSleeping && _dialogAfterSleeping[Save.Data.CurrentDay]) NPC.s_OnNPCInteract?.Invoke(_dialogAfterSleeping[Save.Data.CurrentDay]);
        
    }
}
