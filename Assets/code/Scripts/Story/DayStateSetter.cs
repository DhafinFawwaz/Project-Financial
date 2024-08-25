using UnityEngine;

public class DayStateSetter : MonoBehaviour
{
    public void SetAfterSleeping() => Save.Data.CurrentDayData.State = DayState.AfterSleeping;
    public void SetAfterKuliah() => Save.Data.CurrentDayData.State = DayState.AfterKuliah;
    public void SetAfterPlanning() => Save.Data.CurrentDayData.State = DayState.AfterBudgeting;
    public void SetAfterBelanja() => Save.Data.CurrentDayData.State = DayState.AfterBelanja;
    public void SetAfterStreaming() => Save.Data.CurrentDayData.State = DayState.AfterStreaming;

}