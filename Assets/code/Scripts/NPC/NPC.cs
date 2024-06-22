using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] DialogData _commentList;
    public static Action<DialogData> s_OnNPCInteract;

    protected override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        s_OnNPCInteract?.Invoke(_commentList);
    }
}
