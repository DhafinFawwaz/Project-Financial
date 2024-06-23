using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceObserver : MonoBehaviour
{
    [SerializeField] Dialog _dialog;
    [SerializeField] OptionSession _optionSession;

    void OnEnable()
    {
        NPC.s_OnNPCInteract += _dialog.SetDataAndPlay;
        NPC.s_OnNPCInteract += DisableMouseAndKey;
        Dialog.s_OnDialogFinished += EnableMouseAndKey;
        Rak.s_OnRakInteract += _optionSession.SetDataAndPlay;
    }

    void OnDisable()
    {
        NPC.s_OnNPCInteract -= _dialog.SetDataAndPlay;
        NPC.s_OnNPCInteract -= DisableMouseAndKey;
        Dialog.s_OnDialogFinished -= EnableMouseAndKey;
        Rak.s_OnRakInteract -= _optionSession.SetDataAndPlay;
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
