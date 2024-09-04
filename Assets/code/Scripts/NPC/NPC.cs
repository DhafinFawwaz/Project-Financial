using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] DialogData _commentListJustGotHome;
    [SerializeField] DialogData _commentListAfterStreaming;
    [SerializeField] DialogData _commentListAfterSleeping;
    [SerializeField] DialogData _commentListJustGotOutside;
    [SerializeField] DialogData _commentListAfterKuliah;
    [SerializeField] DialogData _commentListAfterBudgeting;
    [SerializeField] DialogData _commentListAfterBelanja;

    public static Action<DialogData> s_OnNPCInteract;

    protected override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        switch(Save.Data.DayState) {
            case DayState.JustGotHome: s_OnNPCInteract?.Invoke(_commentListJustGotHome); break;
            case DayState.AfterStreaming: s_OnNPCInteract?.Invoke(_commentListAfterStreaming); break;
            case DayState.AfterSleeping: s_OnNPCInteract?.Invoke(_commentListAfterSleeping); break;
            case DayState.JustGotOutside: s_OnNPCInteract?.Invoke(_commentListJustGotOutside); break;
            case DayState.AfterKuliah: s_OnNPCInteract?.Invoke(_commentListAfterKuliah); break;
            case DayState.AfterBudgeting: s_OnNPCInteract?.Invoke(_commentListAfterBudgeting); break;
            case DayState.AfterBelanja: s_OnNPCInteract?.Invoke(_commentListAfterBelanja); break;
        }
    }
}
