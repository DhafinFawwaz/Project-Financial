using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryWorld : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _missionText;
    [SerializeField] GameObject _shopInteractable;
    [SerializeField] GameObject _itbInteractable;
    void Start()
    {
        RefreshStates();
    }

    void RefreshStates()
    {
        if(Save.Data.HasDay1Sleep && Save.Data.CurrentDay == 1)
        {
            _missionText.text = "Ngomong ke Nao dan Riki di depan ITB dulu yuk!.";
            _shopInteractable.SetActive(false);
            _itbInteractable.SetActive(false);
        }   
        if(Save.Data.HasTalkedToNaoRikiInDay2)
        {
            _missionText.text = "Ayo ke ITB!";
            _itbInteractable.SetActive(true);
        }
    }

    public void TalkDone()
    {
        Save.Data.HasTalkedToNaoRikiInDay2 = true;
        RefreshStates();
    }
}
