using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] DialogData _dialogData;

    public void Play()
    {
        this.Invoke(() => {
            NPC.s_OnNPCInteract?.Invoke(_dialogData);
            Dialog.s_OnDialogFinished += OnDialogDone;
        }, 0.05f);
    }

    [SerializeField] UnityEvent _onDialogFinished;
    void OnDialogDone(DialogData data)
    {
        _onDialogFinished?.Invoke();
        Dialog.s_OnDialogFinished -= OnDialogDone;
    }
}
