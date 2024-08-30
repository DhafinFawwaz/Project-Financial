using UnityEngine;

public class DayStateSetter : MonoBehaviour
{
    public void SetJustGotHome() => Save.Data.DayState = DayState.JustGotHome;
    public void SetAfterStreaming() => Save.Data.DayState = DayState.AfterStreaming;
    public void SetAfterSleeping() => Save.Data.DayState = DayState.AfterSleeping;
    public void SetJustGotOutside() => Save.Data.DayState = DayState.JustGotOutside;
    public void SetAfterKuliah() => Save.Data.DayState = DayState.AfterKuliah;
    public void SetAfterBudgeting() => Save.Data.DayState = DayState.AfterBudgeting;
    public void SetAfterBelanja() => Save.Data.DayState = DayState.AfterBelanja;


    public void SetMiauCutsceneDone() => Save.Data.IsMiauCutsceneDone = true;

}