using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] DialogData _commentList;
    [SerializeField] DialogData _commentListAfterKuliah;
    [SerializeField] DialogData _commentListAfterBelanja;
    public static Action<DialogData> s_OnNPCInteract;

    protected override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        if(Save.Data.DayState == DayState.AfterKuliah && _commentListAfterKuliah) s_OnNPCInteract?.Invoke(_commentListAfterKuliah);
        else if(Save.Data.DayState == DayState.AfterBelanja && _commentListAfterBelanja) s_OnNPCInteract?.Invoke(_commentListAfterBelanja);
        else if(Save.Data.DayState == DayState.JustGotOutside && _commentList) s_OnNPCInteract?.Invoke(_commentList);
    }
}
