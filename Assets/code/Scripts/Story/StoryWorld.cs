using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryWorld : MonoBehaviour
{
    [SerializeField] Transform _playerTrans;
    [SerializeField] TextMeshProUGUI _missionText;
    [SerializeField] GameObject _belanjaInteractable;
    [SerializeField] GameObject _tutorialBelanjaInteractable;
    [SerializeField] GameObject _itbInteractable;
    [SerializeField] GameObject _miauInteractable;
    [SerializeField] GameObject _houseInteractable;
    [SerializeField] MissionSetter _talkToShadowInDay5Mission;
    [SerializeField] MissionSetter _talkToShadowInDay6Mission;

    [Header("Day7")]
    [SerializeField] MissionSetter[] _day7MissionAfterKuliah;
    [SerializeField] Collider _apotekDialogTrigger;
    [SerializeField] Collider _belanjaDialogTrigger;
    [SerializeField] GameObject _naoAfterObatDibeli;
    [SerializeField] NPC _naoDepanKos;
    [SerializeField] NPC _rikiDepanKosNaoTrigger;

    void Start()
    {
        _playerTrans.position = Save.Data.Position;
        RefreshStates();
    }

    public void RefreshStates()
    {
        if(Save.Data.DayState == DayState.JustGotOutside)
        {
            _missionText.text = "Ayo ke ITB!";
            _belanjaInteractable.SetActive(false);
            _itbInteractable.SetActive(true);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(false);


            if(!Save.Data.CurrentDayData.HasTalkedToNaoRiki)
            {
                _missionText.text = "Ngomong ke Nao dan Riki";
                _belanjaInteractable.SetActive(false);
                _itbInteractable.SetActive(false);
                _miauInteractable.SetActive(false);
                _houseInteractable.SetActive(false);
            }
        }


        if(Save.Data.DayState == DayState.AfterBudgeting || Save.Data.DayState == DayState.AfterKuliah)
        {
            _missionText.text = "Belanja di Bunga Mart Yuk!.";
            if(Save.Data.CurrentDay <= 2) {
                _belanjaInteractable.SetActive(false);
                _tutorialBelanjaInteractable.SetActive(true);
            } else {
                _belanjaInteractable.SetActive(true);
                _tutorialBelanjaInteractable.SetActive(false);
            }
            _itbInteractable.SetActive(false);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(false);


            if(Save.Data.CurrentDay == 4 && !Save.Data.HasTalkedToShadowInDay5) {
                _belanjaInteractable.gameObject.SetActive(false);
                _tutorialBelanjaInteractable.gameObject.SetActive(false);
                _itbInteractable.gameObject.SetActive(false);
                _miauInteractable.gameObject.SetActive(false);
                _houseInteractable.gameObject.SetActive(false);
                this.Invoke(_talkToShadowInDay5Mission.Set, 0.06f);
            }

            if(Save.Data.CurrentDay == 5 && !Save.Data.HasTalkedToShadowInDay6) {
                _belanjaInteractable.gameObject.SetActive(false);
                _tutorialBelanjaInteractable.gameObject.SetActive(false);
                _itbInteractable.gameObject.SetActive(false);
                _miauInteractable.gameObject.SetActive(false);
                _houseInteractable.gameObject.SetActive(false);
                this.Invoke(_talkToShadowInDay6Mission.Set, 0.06f);
            }


            _rikiDepanKosNaoTrigger.enabled = false;
            _naoDepanKos.enabled = false;
            if(Save.Data.CurrentDay == 6 && Save.Data.Day7State < _day7MissionAfterKuliah.Length) {
                _belanjaInteractable.gameObject.SetActive(false);
                _tutorialBelanjaInteractable.gameObject.SetActive(false);
                _itbInteractable.gameObject.SetActive(false);
                _miauInteractable.gameObject.SetActive(false);
                _houseInteractable.gameObject.SetActive(false);
                // _apotekDialogTrigger.gameObject.SetActive(false);
                // _belanjaDialogTrigger.gameObject.SetActive(false);
                _apotekDialogTrigger.enabled = false;
                _belanjaDialogTrigger.enabled = false;
                _naoAfterObatDibeli.gameObject.SetActive(false);
                
                this.Invoke(_day7MissionAfterKuliah[Save.Data.Day7State].Set, 0.06f);
                if(Save.Data.Day7State == 0) {
                    _rikiDepanKosNaoTrigger.enabled = true;
                } else if(Save.Data.Day7State == 2) {
                    // _apotekDialogTrigger.gameObject.SetActive(true);
                    _apotekDialogTrigger.enabled = true;
                } else if(Save.Data.Day7State == 3) {
                    _naoDepanKos.gameObject.SetActive(false);
                    _naoAfterObatDibeli.gameObject.SetActive(true);
                } 
            } else {
                _belanjaDialogTrigger.gameObject.SetActive(true);
                _belanjaDialogTrigger.enabled = true;
            }
        }

        if(Save.Data.DayState == DayState.AfterBelanja)
        {
            _missionText.text = "Capek, pulang streaming yuk!";
            _belanjaInteractable.SetActive(false);
            _itbInteractable.SetActive(false);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(true);
            
            if(Save.Data.CurrentDay == 1 && !Save.Data.IsMiauCutsceneDone)
            {
                _belanjaInteractable.SetActive(false);
                _itbInteractable.SetActive(false);
                _miauInteractable.SetActive(true);
                _houseInteractable.SetActive(false);
            }
        }

    }

    public void TalkDone()
    {
        Save.Data.CurrentDayData.HasTalkedToNaoRiki = true;
        RefreshStates();
    }

    public void TalkedToShadowInDay5()
    {
        Save.Data.HasTalkedToShadowInDay5 = true;
        RefreshStates();
    }

    public void TalkedToShadowInDay6()
    {
        Save.Data.HasTalkedToShadowInDay6 = true;
        RefreshStates();
    }

    public void IncrementDay7State()
    {
        Save.Data.Day7State++;
        RefreshStates();
    }
}
