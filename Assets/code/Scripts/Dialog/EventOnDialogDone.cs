using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnDialogDone : MonoBehaviour
{
    [SerializeField] UnityEvent _onDialogStarted;
    [SerializeField] UnityEvent _onDialogFinished;
    void OnEnable()
    {
        Dialog.s_OnDialogStart += OnDialogStart;
        Dialog.s_OnDialogFinished += OnDialogDone;
    }

    void OnDisable()
    {
        Dialog.s_OnDialogStart -= OnDialogStart;
        Dialog.s_OnDialogFinished -= OnDialogDone;
    }

    void OnDialogDone(DialogData data)
    {
        _onDialogFinished?.Invoke();
    }

    void OnDialogStart(DialogData data)
    {
        _onDialogStarted?.Invoke();
    }
}
