using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioObserver : MonoBehaviour
{
    [SerializeField] AudioManager _audio;
    void OnEnable()
    {
        ButtonUI.s_onClick += ButtonOnClick;
        ButtonUI.s_onPointerEnter += ButtonEnter;
        ButtonUI.s_onPointerDown += ButtonDown;
        ButtonUI.s_onSelect += ButtonEnter;

        AnimationUI.OnPlaySoundByFile += _audio.PlaySound;
        AnimationUI.OnPlaySoundByIndex += _audio.PlaySound;

        SceneTransition.s_onBeforeOut += MusicFadeOut;
        SceneTransition.s_onAfterIn += SetMusicSourceVolumeToDefault;

        Rak.s_OnRakUnlocked += SFXRakUnlock;
        Interactable.s_OnLabelShown += SFXLabelShown;
        Shadow.s_OnShadowHurt += SFXOnShadowHurt;
        PlayerKerasukan.s_OnKerasukanStart += SFXOnKerasukanStart;
        PlayerKerasukan.s_OnKerasukanEnd += SFXOnKerasukanEnd;
        PlayerCore.s_OnCollect += SFXOnCollect;
        SpacebarSpam.s_OnSpacebarSpamDown += SFXSpacebarSpamDown;
        FootstepPlayer.s_OnFootstep += SFXFootstep;
        PopUp.s_OnPopUpShow += SFXPopUpShow;
        PopUp.s_OnPopUpHide += SFXPopUpHide;
        Flashlight.s_OnNotEnoughEnergy += SFXNotEnoughEnergy;
        FlashLightUI.s_OnRechargingUI += SFXCharging;
        Flashlight.s_OnFlashlightOn += SFXFlashlightOn;
        MoneyTransferAnimation.s_OnMoneySpawn += SFXMoneySpawn;
        FiscalGuardian.s_OnWin += SFXWin;
        FiscalGuardian.s_OnLose += SFXLose;
        BungaShooter.s_OnWin += SFXWin;
        BungaShooter.s_OnLose += SFXLose;
        PieChartItemSpawner.s_OnItemPieThrown += SFXOnItemPieThrown;
        StreamingManager.s_OnIncreaseViews += SFXWin;
        StreamingManager.s_OnDecreaseViews += SFXLose;
    }
    void OnDisable()
    {
        ButtonUI.s_onClick -= ButtonOnClick;
        ButtonUI.s_onPointerEnter -= ButtonEnter;
        ButtonUI.s_onPointerDown -= ButtonDown;
        ButtonUI.s_onSelect -= ButtonEnter;

        AnimationUI.OnPlaySoundByFile -= _audio.PlaySound;
        AnimationUI.OnPlaySoundByIndex -= _audio.PlaySound;

        SceneTransition.s_onBeforeOut -= MusicFadeOut;
        SceneTransition.s_onAfterIn -= SetMusicSourceVolumeToDefault;

        Rak.s_OnRakUnlocked -= SFXRakUnlock;
        Interactable.s_OnLabelShown -= SFXLabelShown;
        Shadow.s_OnShadowHurt -= SFXOnShadowHurt;
        PlayerKerasukan.s_OnKerasukanStart -= SFXOnKerasukanStart;
        PlayerKerasukan.s_OnKerasukanEnd -= SFXOnKerasukanEnd;
        PlayerCore.s_OnCollect -= SFXOnCollect;
        SpacebarSpam.s_OnSpacebarSpamDown -= SFXSpacebarSpamDown;
        FootstepPlayer.s_OnFootstep -= SFXFootstep;
        PopUp.s_OnPopUpShow -= SFXPopUpShow;
        PopUp.s_OnPopUpHide -= SFXPopUpHide;
        Flashlight.s_OnNotEnoughEnergy -= SFXNotEnoughEnergy;
        FlashLightUI.s_OnRechargingUI -= SFXCharging;
        Flashlight.s_OnFlashlightOn -= SFXFlashlightOn;
        MoneyTransferAnimation.s_OnMoneySpawn -= SFXMoneySpawn;
        FiscalGuardian.s_OnWin -= SFXWin;
        FiscalGuardian.s_OnLose -= SFXLose;
        BungaShooter.s_OnWin -= SFXWin;
        BungaShooter.s_OnLose -= SFXLose;
        PieChartItemSpawner.s_OnItemPieThrown -= SFXOnItemPieThrown;
        StreamingManager.s_OnIncreaseViews -= SFXWin;
        StreamingManager.s_OnDecreaseViews -= SFXLose;
    }

    void SFXOnItemPieThrown() => _audio.PlaySound(22);

    void SFXWin() => _audio.PlaySound(11);
    void SFXLose() => _audio.PlaySound(17);

    void SFXMoneySpawn() => _audio.PlaySound(22);

    void SFXCharging(float progress) {
        float pitch = remap(progress, 0, 1, -3, 3);
        _audio.PlaySoundPitch(13, pitch);
    }
    float remap(float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

    void SFXNotEnoughEnergy() => _audio.PlaySound(21);
    void SFXFlashlightOn() => _audio.PlaySound(18);


    void SFXPopUpShow() => _audio.PlaySound(27);
    void SFXPopUpHide() => _audio.PlaySound(23);
    void SFXRakUnlock() => _audio.PlaySound(20);
    void SFXLabelShown() => _audio.PlaySound(30);
    void SFXOnShadowHurt() => _audio.PlaySound(Random.Range(7, 10));
    void SFXOnKerasukanStart() => _audio.PlaySound(10);
    void SFXOnKerasukanEnd() => _audio.PlaySound(5);
    void SFXOnCollect() => _audio.PlaySound(25);
    void SFXSpacebarSpamDown() => _audio.PlaySound(6);
    void SFXFootstep() => _audio.PlaySoundRandomPitch(31, 0, 1);




    void MusicFadeOut() => _audio.MusicFadeOut(0.5f);
    void SetMusicSourceVolumeToDefault() => _audio.SetMusicSourceVolume(1);

    

    void ButtonEnter() => _audio.PlaySound(3);
    void ButtonOnClick() => _audio.PlaySound(2);
    void ButtonDown() => _audio.PlaySound(0);
}