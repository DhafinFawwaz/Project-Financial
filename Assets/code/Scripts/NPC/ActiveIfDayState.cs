using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveIfDayState : MonoBehaviour
{
    [SerializeField] bool _justGotHome = true;
    [SerializeField] bool _afterStreaming = true;
    [SerializeField] bool _afterSleeping = true;
    [SerializeField] bool _justGotOutside = true;
    [SerializeField] bool _afterKuliah = true;
    [SerializeField] bool _afterBudgeting = true;
    [SerializeField] bool _afterBelanja = true;

    void Awake()
    {
        switch(Save.Data.DayState) {
            case DayState.JustGotHome: gameObject.SetActive(_justGotHome); break;
            case DayState.AfterStreaming: gameObject.SetActive(_afterStreaming); break;
            case DayState.AfterSleeping: gameObject.SetActive(_afterSleeping); break;
            case DayState.JustGotOutside: gameObject.SetActive(_justGotOutside); break;
            case DayState.AfterKuliah: gameObject.SetActive(_afterKuliah); break;
            case DayState.AfterBudgeting: gameObject.SetActive(_afterBudgeting); break;
            case DayState.AfterBelanja: gameObject.SetActive(_afterBelanja); break;
        }
    }
}
