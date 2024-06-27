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


public enum DialogActor
{
    Angkasa,
    Nao,
    PakRudi,
    PakHendra,
    Nadine,
    Miau,
    MasDanang,
    Fajar,
    BuYulianti,
    BuRatna,
    Bima,
    BerandalMainJudi,
    Anaya,
    Riki,
    Kunti
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
            case DialogActor.PakRudi: return "Pak Rudi";
            case DialogActor.PakHendra: return "Pak Hendra";
            case DialogActor.Nadine: return "Nadine";
            case DialogActor.Miau: return "Miau";
            case DialogActor.MasDanang: return "Mas Danang";
            case DialogActor.Fajar: return "Fajar";
            case DialogActor.BuYulianti: return "Bu Yulianti";
            case DialogActor.BuRatna: return "Bu Ratna";
            case DialogActor.Bima: return "Bima";
            case DialogActor.BerandalMainJudi: return "Berandal Main Judi";
            case DialogActor.Anaya: return "Anaya";
            case DialogActor.Riki: return "Riki";
            case DialogActor.Kunti: return "Kunti";
        }
        return "Angkasa";
    }
    // [SerializeField] DialogActor _actorLeft;
    [SerializeField] DialogActor _actorRight;
    public DialogActor DialogActor => _actorRight;
    [SerializeField] SusLevel _susLevel;
    public SusLevel SusLevel => _susLevel;


    [TextArea]
    [SerializeField] string _text;
}
