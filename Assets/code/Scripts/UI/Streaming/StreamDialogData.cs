using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamDialogData : ScriptableObject
{
    [SerializeField] StreamDialog[] _streamDialogs;
}

public class StreamDialog
{
    enum DialogType
    {
        Streamer,
        Watcher
    }
    [SerializeField] DialogType _dialogType;

    [SerializeField] string _title = "";
    [SerializeField] string _message = "";
}