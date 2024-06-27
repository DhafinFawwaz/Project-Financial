using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXOnSceneTrigger : MonoBehaviour
{
    [SerializeField] SceneTrigger _sceneTrigger;
    [SerializeField] AudioClip _sfx;
    [SerializeField] int _sfxIndex;
    public void PlaySFX()
    {
        Audio.PlaySound(_sfx);
    }

    public void PlaySFXByIndex()
    {
        Audio.PlaySound(_sfxIndex);
    }
}
