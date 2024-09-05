using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] DialogData _dialogData;

    public void Play()
    {
        this.Invoke(() => {
            NPC.s_OnNPCInteract?.Invoke(_dialogData);
        }, 0.05f);
    }
}
