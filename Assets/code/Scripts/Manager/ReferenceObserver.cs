using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceObserver : MonoBehaviour
{
    [SerializeField] Dialog _dialog;

    void OnEnable()
    {
        NPC.s_OnNPCInteract += _dialog.SetDataAndPlay;
        NPC.s_OnNPCInteract += DisableMouseAndKey;
        Dialog.s_OnDialogFinished += EnableMouseAndKey;
    }

    void OnDisable()
    {
        NPC.s_OnNPCInteract -= _dialog.SetDataAndPlay;
        NPC.s_OnNPCInteract -= DisableMouseAndKey;
        Dialog.s_OnDialogFinished -= EnableMouseAndKey;
    }

    void EnableMouseAndKey(DialogData _)
    {
        InputManager.SetActiveMouseAndKey(true);
    }

    void DisableMouseAndKey(DialogData _)
    {
        InputManager.SetActiveMouseAndKey(false);
    }
}
