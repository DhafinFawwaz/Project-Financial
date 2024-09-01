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
    public const int INCREASE_STOCK_EVERY = 3;
    const int MAX_DAY = 15;
    public long NeedsMoney = 100000;
    public long DesireMoney = 30000;
    public long DebitMoney = 40000;
    public long DebitTabunganMoney = 150000;
    public long TotalMoney => NeedsMoney + DesireMoney + DebitMoney;
    

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
        Happiness = 100;
        Health = 100;
        CurrentDay = 2;
        CurrentListBelanja = new List<ItemCount>();
        CurrentBelanjaMoney = 0;
        SkillPoin = 0;
        SubscriberAmount = 50000;

        NeedsMoney = 100000;
        DesireMoney = 30000;
        DebitMoney = 40000;
        DebitTabunganMoney = 150000;

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
        public long CreditMoney = 50000;
        public long GainedMoney = 100000;

        public DayData()
        {
            StreamingCounter = 0;
            GainedSubscriber = 0;
            CreditMoney = 50000;
            GainedMoney = 100000;
        }
    }


    const int GET_MONEY_EVERY = 3;
    public long TotalGainedMoneyFromToday(){
        int day = CurrentDay;
        long total = 0;
        for(int i = day-GET_MONEY_EVERY+1; i <= day; i++){
            total += DayDatas[i].GainedMoney;
        }
        return total;
    }

    public List<long> GainedSubscriberEachDay => DayDatas.ConvertAll(x => x.GainedSubscriber);

    public List<DayData> DayDatas = new List<DayData>();

    public DayData CurrentDayData => DayDatas[CurrentDay];

    // Story booleans
    public bool HasTalkedToNaoRikiInDay2 = false;

    // After kuliah
    public bool HasDoneKuliah = false;
    public bool JustAfterFirstBelanja = false;
    public bool IsMiauCutsceneDone = false;

    public bool GameEnd = false;



    public static int DaysUntilNewStock()
    {
        int d = Save.Data.CurrentDay;
        // hard coded lol
        switch(d) {
            case 0: return 2;
            case 1: return 1;
            case 2: return 3;
            case 3: return 2;
            case 4: return 1;
            case 5: return 3;
            case 6: return 2;
            case 7: return 1;
            case 8: return 3;
            case 9: return 2;
            case 10: return 1;
            case 11: return 3;
            case 12: return 2;
            case 13: return 1;
            case 14: return 3;
            default: return 0;
        }
    }

}
