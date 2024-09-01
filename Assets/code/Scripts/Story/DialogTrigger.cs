using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] DialogData _dialogData;

    public void Play()
    {
        NPC.s_OnNPCInteract?.Invoke(_dialogData);
    }
}
