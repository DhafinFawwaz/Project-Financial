using System.Collections.Generic;
using UnityEngine;
public enum DayState
{
    AfterSleeping,
    AfterKuliah,
    AfterBudgeting,
    AfterBelanja,
    AfterStreaming,
}

[System.Serializable]
public class SaveData
{
    const int MAX_DAY = 15;
    public long Money = 100000;

    double _happiness = 100;
    public double Happiness { get => _happiness; set => _happiness = Mathf.Clamp((float)value, 0, 100); }

    double _health = 100;
    public double Health { get => _health; set => _health = Mathf.Clamp((float)value, 0, 100); }
    public int SkillPoin = 0;
    public int CurrentDay = 0;
    public long SubscriberAmount = 50000;

    public List<ItemCount> CurrentListBelanja = new List<ItemCount>();
    public long CurrentBelanjaMoney = 0;
    
    public SaveData()
    {
        Money = 100000;
        Happiness = 100;
        Health = 100;
        CurrentDay = 0;
        CurrentListBelanja = new List<ItemCount>();
        CurrentBelanjaMoney = 0;
        SkillPoin = 0;
        SubscriberAmount = 50000;

        for(int i = 0; i < MAX_DAY; i++)
        {
            DayDatas.Add(new DayData());
        }

        HasDoneKuliah = false;
        JustAfterFirstBelanja = false;
    }

    

    [System.Serializable]
    public class DayData
    {
        public long StreamingCounter = 0;
        public long GainedSubscriber = 0;
        public DayState State = DayState.AfterBelanja;
    }

    public List<long> GainedSubscriberEachDay => DayDatas.ConvertAll(x => x.GainedSubscriber);

    public List<DayData> DayDatas = new List<DayData>();

    public DayData CurrentDayData => DayDatas[CurrentDay];

    // Story booleans
    public bool HasTalkedToNaoRikiInDay2 = false;

    // After kuliah
    public bool HasDoneKuliah = false;
    public bool JustAfterFirstBelanja = false;

    public bool GameEnd = false;

}
