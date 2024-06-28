using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryBedroom : MonoBehaviour
{
    [SerializeField] GameObject _streamingInteractable;
    [SerializeField] GameObject _keluarBlocker;
    [SerializeField] GameObject _kasurInteractable;
    [SerializeField] DialogTrigger _dialogTrigger;
    [SerializeField] TextMeshProUGUI _startDate;
    [SerializeField] TextMeshProUGUI _endDate;
    [SerializeField] TextMeshProUGUI _missionText;
    void Awake()
    {
        RefreshStates();
    }

    public void RefreshStates()
    {
        if (Save.Data.StreamingCounter[Save.Data.CurrentDay] == 3)
        {
            _missionText.text = "Udah woi jangan Streaming lagi, waktunya tidur!";
            _streamingInteractable.SetActive(false);
        }
        else
        {
            if(Save.Data.StreamingCounter[0] == 0)
            {
                _missionText.text = "Waktunya Streaming!";
                _streamingInteractable.SetActive(true);
            }
        }

        if(Save.Data.StreamingCounter[Save.Data.CurrentDay] == 1)
        {
            _missionText.text = "Waktunya tidur, atau streaming lagi hehe.";
            _kasurInteractable.SetActive(true);
        } 
        else 
        {
            _kasurInteractable.SetActive(false);
        }

        if(Save.Data.CurrentDay >= 1)
        {
            _missionText.text = "Ayo kuliah ke ITB!";
            _keluarBlocker.SetActive(false);
            _streamingInteractable.SetActive(false);
        }   
        else
        {
            _keluarBlocker.SetActive(true);
        }

        if(Save.Data.CurrentDay == 0 && Save.Data.StreamingCounter[Save.Data.CurrentDay] == 0)
        {
            Invoke("TriggerDialog", 0.1f);
        }
    }

    void TriggerDialog()
    {
        _dialogTrigger.Play();
    }

    public void IncrementDay()
    {
        _startDate.text = (Save.Data.CurrentDay+1).ToString();
        _endDate.text = (Save.Data.CurrentDay+2).ToString();
        Save.Data.CurrentDay++;
        Debug.Log(Save.Data.CurrentDay);

        if(Save.Data.CurrentDay == 1)
        {
            Save.Data.HasDay1Sleep = true;
        }
    }

}
