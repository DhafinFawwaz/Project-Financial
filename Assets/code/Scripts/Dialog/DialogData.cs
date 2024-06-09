using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "DialogData", order = 1)]
public class DialogData : ScriptableObject
{
    public DialogContent[] Content => _content;
    [SerializeField] DialogContent[] _content;
}


enum DialogActor
{
    Angkasa,
    Nao
}

public enum SusLevel
{
    Angkasa,
    Baik,
    Sus
}

[System.Serializable]
public class DialogContent
{
    // public string ActorLeft => getString(_actorLeft);
    public string ActorRight => getString(_actorRight);

    public string Text => _text;

    
    string getString(DialogActor _actor)
    {
        switch(_actor)
        {
            case DialogActor.Angkasa: return "Angkasa";
            case DialogActor.Nao: return "Nao";
        }
        return "Angkasa";
    }
    // [SerializeField] DialogActor _actorLeft;
    [SerializeField] DialogActor _actorRight;
    [SerializeField] SusLevel _susLevel;
    public SusLevel SusLevel => _susLevel;

    [TextArea]
    [SerializeField] string _text;
}
