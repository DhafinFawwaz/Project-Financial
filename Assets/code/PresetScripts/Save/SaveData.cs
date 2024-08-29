using System.Collections.Generic;
using UnityEngine;
public enum DayState
{
    JustGotHome,
    AfterStreaming,
    AfterSleeping,
    JustGotOutside,
    AfterKuliah,
    AfterBudgeting,
    AfterBelanja,
}

[System.Serializable]
public class SaveData
{
    const int MAX_DAY = 15;
    public long CashMoney = 100000;
    public long DesireMoney = 0;
    public long DebitMoney = 0;
    public long CreditMoney = 0;
    

    double _happiness = 100;
    public double Happiness { get => _happiness; set => _happiness = Mathf.Clamp((float)value, 0, 100); }

    double _health = 100;
    public double Health { get => _health; set => _health = Mathf.Clamp((float)value, 0, 100); }
    public int SkillPoin = 0;
    public int CurrentDay = 0;
    public long SubscriberAmount = 50000;

    public List<ItemCount> CurrentListBelanja = new List<ItemCount>();
    public long CurrentBelanjaMoney = 0;
    public Vector3 Position = Vector3.zero;

    // Shop Stocks
    public List<int> HealthItemStocks = new List<int>();
    public List<int> HapinessItemStocks = new List<int>();

    public DayState DayState = DayState.JustGotHome;
    
    public SaveData()
    {
        CashMoney = 100000;
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

        HealthItemStocks = new List<int>(){3,3,3,3};
        HapinessItemStocks = new List<int>(){3,3,3,3};

        DayState = DayState.JustGotHome;
    }

    

    [System.Serializable]
    public class DayData
    {
        public long StreamingCounter = 0;
        public long GainedSubscriber = 0;
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
