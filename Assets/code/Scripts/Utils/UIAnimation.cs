using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIAnimation : MonoBehaviour
{
    [SerializeField] UIAnimation[] _animationToInterrupt;

    protected void StopAllOtherGraphics()
    {
        foreach (var g in _animationToInterrupt)
            g.Stop();
    }

    public abstract void Stop();
    
}