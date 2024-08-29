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

    void RefreshStates()
    {
        if(Save.Data.CurrentDay == 1)
        {
            _missionText.text = "Ngomong ke Nao dan Riki di depan ITB dulu yuk!.";
            _shopInteractable.SetActive(false);
            _itbInteractable.SetActive(false);
        }   
        if(Save.Data.HasTalkedToNaoRikiInDay2)
        {
            _missionText.text = "Ayo ke ITB!";
            _itbInteractable.SetActive(true);
            _shopInteractable.SetActive(false);
        }


        if(Save.Data.DayState == DayState.AfterBudgeting)
        {
            _missionText.text = "Belanja di Bunga Mart Yuk!.";
            _itbInteractable.gameObject.SetActive(false);
            _shopInteractable.SetActive(true);
        }

        if(Save.Data.JustAfterFirstBelanja)
        {
            _missionText.text = "Capek, streaming atau tidur yuk!";
            _itbInteractable.gameObject.SetActive(false);
            _shopInteractable.SetActive(false);
            _miauInteractable.SetActive(true);
            _houseInteractable.SetActive(false);
        }
    }

    public void TalkDone()
    {
        Save.Data.HasTalkedToNaoRikiInDay2 = true;
        RefreshStates();
    }
}
