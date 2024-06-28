using System;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] DialogData _commentList;

    public void Play()
    {
        NPC.s_OnNPCInteract?.Invoke(_commentList);
    }
}
