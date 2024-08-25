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
        if (Save.Data.CurrentDayData.StreamingCounter == 3)
        {
            _missionText.text = "Udah woi jangan Streaming lagi, waktunya tidur!";
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(true);
        }

        if(Save.Data.CurrentDayData.StreamingCounter == 1 || Save.Data.CurrentDayData.StreamingCounter == 2)
        {
            _missionText.text = "Waktunya tidur, atau streaming lagi hehe.";
            _streamingInteractable.SetActive(true);
            _kasurInteractable.SetActive(true);
        } 

        if(Save.Data.CurrentDayData.StreamingCounter == 0 && Save.Data.CurrentDayData.State == DayState.AfterBelanja)
        {
            _missionText.text = "Waktunya Streaming!";
            _streamingInteractable.SetActive(true);
            _kasurInteractable.SetActive(false);
        }

        if(Save.Data.CurrentDayData.State == DayState.AfterSleeping)
        {
            _missionText.text = "Ayo kuliah ke ITB!";
            _keluarBlocker.SetActive(false);
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(false);
        }   

        if(Save.Data.CurrentDayData.State != DayState.AfterSleeping)
        {
            _keluarBlocker.SetActive(true);
        }

        if(Save.Data.CurrentDay == 0 && Save.Data.CurrentDayData.State == DayState.AfterBelanja)
        {
            this.Invoke(() => _dialogTrigger.Play(), 0.1f);
        }
    }

    public void IncrementDay()
    {
        _startDate.text = (Save.Data.CurrentDay+1).ToString();
        _endDate.text = (Save.Data.CurrentDay+2).ToString();
        Save.Data.CurrentDay++;
    }

}
