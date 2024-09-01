using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXOnSceneTrigger : MonoBehaviour
{
    [SerializeField] int _sfxIndex;
    public void PlaySFX()
    {
        Audio.PlaySound(_sfxIndex);
    }

    public void PlaySFXByIndex()
    {
        Audio.PlaySound(_sfxIndex);
    }
}
