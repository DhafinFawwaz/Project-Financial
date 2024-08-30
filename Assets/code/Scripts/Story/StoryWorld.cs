using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryWorld : MonoBehaviour
{
    [SerializeField] Transform _playerTrans;
    [SerializeField] TextMeshProUGUI _missionText;
    [SerializeField] GameObject _shopInteractable;
    [SerializeField] GameObject _itbInteractable;
    [SerializeField] GameObject _miauInteractable;
    [SerializeField] GameObject _houseInteractable;
    void Start()
    {
        _playerTrans.position = Save.Data.Position;
        RefreshStates();
    }

    public void RefreshStates()
    {
        if(Save.Data.CurrentDay == 1 && !Save.Data.HasTalkedToNaoRikiInDay2)
        {
            _missionText.text = "Ngomong ke Nao dan Riki di depan ITB dulu yuk!.";
            _shopInteractable.SetActive(false);
            _itbInteractable.SetActive(false);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(true);
        }   
        else if(Save.Data.DayState == DayState.JustGotOutside)
        {
            _missionText.text = "Ayo ke ITB!";
            _shopInteractable.SetActive(false);
            _itbInteractable.SetActive(true);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(true);
        }


        if(Save.Data.DayState == DayState.AfterBudgeting)
        {
            _missionText.text = "Belanja di Bunga Mart Yuk!.";
            _shopInteractable.SetActive(true);
            _itbInteractable.SetActive(false);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(false);
        }

        if(Save.Data.DayState == DayState.AfterBelanja)
        {
            _missionText.text = "Capek, pulang streaming yuk!";
            _shopInteractable.SetActive(false);
            _itbInteractable.SetActive(false);
            _miauInteractable.SetActive(false);
            _houseInteractable.SetActive(true);
            
            if(Save.Data.CurrentDay == 2 && !Save.Data.IsMiauCutsceneDone)
            {
                _shopInteractable.SetActive(false);
                _itbInteractable.SetActive(false);
                _miauInteractable.SetActive(true);
                _houseInteractable.SetActive(false);
            }
        }

    }

    public void TalkDone()
    {
        Save.Data.HasTalkedToNaoRikiInDay2 = true;
        RefreshStates();
    }
}
