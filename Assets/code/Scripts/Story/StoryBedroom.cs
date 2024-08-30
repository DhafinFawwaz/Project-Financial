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
    [SerializeField] TextMeshProUGUI _startDate2;
    [SerializeField] TextMeshProUGUI _endDate;
    [SerializeField] TextMeshProUGUI _missionText;
    void Awake()
    {
        RefreshStates();
    }

    public void RefreshStates()
    {
        // Special case
        if(Save.Data.DayState == DayState.AfterBelanja)
        {
            Save.Data.DayState = DayState.JustGotHome;
        }



        if (Save.Data.CurrentDayData.StreamingCounter == 3)
        {
            _missionText.text = "Udah woi jangan Streaming lagi, waktunya tidur!";
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(true);
            _keluarBlocker.SetActive(true);
        }
        else if(Save.Data.CurrentDayData.StreamingCounter == 1 || Save.Data.CurrentDayData.StreamingCounter == 2)
        {
            _missionText.text = "Waktunya tidur, atau streaming lagi hehe.";
            _streamingInteractable.SetActive(true);
            _kasurInteractable.SetActive(true);
            _keluarBlocker.SetActive(true);
        } 

        else if(Save.Data.CurrentDayData.StreamingCounter == 0 && Save.Data.DayState == DayState.JustGotHome)
        {
            _missionText.text = "Waktunya Streaming!";
            _streamingInteractable.SetActive(true);
            _kasurInteractable.SetActive(false);
            _keluarBlocker.SetActive(true);
            if(Save.Data.CurrentDay == 0 && Save.Data.DayState == DayState.JustGotHome)
            {
                this.Invoke(() => _dialogTrigger.Play(), 0.1f);
            }
        }

        else if(Save.Data.DayState == DayState.AfterSleeping)
        {
            _missionText.text = "Ayo kuliah ke ITB!";
            _keluarBlocker.SetActive(false);
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(false);
        }   

        
        else if(Save.Data.DayState == DayState.JustGotOutside)
        {
            _missionText.text = "Ngomong ke Nao dan Riki di depan ITB dulu yuk!";
            _keluarBlocker.SetActive(false);
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(false);
        }
        else if(Save.Data.DayState == DayState.AfterKuliah || Save.Data.DayState == DayState.AfterBudgeting)
        {
            _missionText.text = "Belanja dulu yuk!";
            _keluarBlocker.SetActive(false);
            _streamingInteractable.SetActive(false);
            _kasurInteractable.SetActive(false);
        }
    }

    public void IncrementDay()
    {
        _startDate.text = (Save.Data.CurrentDay+1).ToString();
        _startDate2.text = (Save.Data.CurrentDay+1).ToString();
        _endDate.text = (Save.Data.CurrentDay+2).ToString();
        Save.Data.CurrentDay++;
        ApplyDayState();
    }

    [SerializeField] DayData _dayData;
    void ApplyDayState()
    {
        // lol hard coded i dont want to mess this up
        int d = Save.Data.CurrentDay;
        if(d == 1) ApplyStocks(0);
        else if(d == 2) ApplyStocks(1);
        else if(d == 5) ApplyStocks(2);
        else if(d == 8) ApplyStocks(3);
        else if(d == 11) ApplyStocks(4);
        else if(d == 14) ApplyStocks(5);
    }
    void ApplyStocks(int _dayIndex)
    {
        Save.Data.HapinessItemStocks = _dayData.State[_dayIndex].HapinessItemStocks;
        Save.Data.HealthItemStocks = _dayData.State[_dayIndex].HealthItemStocks;
    }

}
