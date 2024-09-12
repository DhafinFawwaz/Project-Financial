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
    // Increment this when you change the structure of SaveData
    public const int Version = 1;
    public int CurrentVersion = -1;
    public const long INITIAL_SUBSCRIBER = 50000;

    public const int INCREASE_STOCK_EVERY = 3;
    public const int KREDIT_DEADLINE = 3;


    const int MAX_DAY = 15;
    public long NeedsMoney = 100000;
    public long CurrentNeedsMoney {
        get => _currentNeedsMoney;
        set {
            _currentNeedsMoney = value;
            TempNeedsMoney = value;
        }
    }
    public long _currentNeedsMoney = 100000;

    // used in belanja
    public long TempNeedsMoney;


    public long DesireMoney = 60000;
    public long DebitMoney = 40000;
    public long DebitTabunganMoney = 150000;
    public long TotalMoney => NeedsMoney + DesireMoney + DebitMoney;
    

    double _happiness = 100;
    public double Happiness { get => _happiness; set => _happiness = Mathf.Clamp((float)value, 0, 100); }

    double _health = 100;
    public double Health { get => _health; set => _health = Mathf.Clamp((float)value, 0, 100); }
    public int SkillPoints = 0;
    public int CurrentDay = 0;
    public long SubscriberAmount { 
        get {
            long total = INITIAL_SUBSCRIBER;
            for(int i = 0; i <= CurrentDay; i++)
            {
                total += DayDatas[i].GainedSubscriber;
            }
            return total;
        }
        set {
            CurrentDayData.GainedSubscriber = value;
        }
    }

    public List<ItemCount> CurrentListBelanja = new List<ItemCount>();
    public long CurrentBelanjaMoney = 0;
    public Vector3 Position = Vector3.zero;

    // Shop Stocks
    public List<int> HealthItemStocks = new List<int>();
    public List<int> HapinessItemStocks = new List<int>();

    public DayState DayState = DayState.JustGotHome;


    // Budgeting
    public int CurrentPredictedHealth;
    public int CurrentPredictedHappiness;

    // Belanja
    public int CurrentQuality;
    public int CurrentTotalItems;


    // Streaming
    public int MaxStreamingAmountPerDay = 3;
    
    public SaveData()
    {
        CurrentVersion = Version;
        Happiness = 90;
        Health = 100;
        CurrentDay = 0;
        CurrentListBelanja = new List<ItemCount>();
        CurrentBelanjaMoney = 0;
        SkillPoints = 1;


        NeedsMoney = 200000;
        DesireMoney = 60000;
        DebitMoney = 80000;
        DebitTabunganMoney = 150000;

        CurrentNeedsMoney = NeedsMoney;

        for(int i = 0; i < MAX_DAY; i++)
        {
            DayDatas.Add(new DayData());
        }

        HasDoneKuliah = false;
        JustAfterFirstBelanja = false;

        HealthItemStocks = new List<int>(){1,2,3,0};
        HapinessItemStocks = new List<int>(){1,2,3};

        DayState = DayState.JustGotHome;


        CurrentPredictedHappiness = UnityEngine.Random.Range(10, 100);
        CurrentPredictedHealth = UnityEngine.Random.Range(10, 100);

        MaxStreamingAmountPerDay = 3;

        IsPinjol = false;
        IsLose = false;

        KursiLevel = 0;
        MonitorLevel = 0;
        OtherLevel = 0;

        IsBungaShooterBought = false;

        KursiLevel = 0;
        MonitorLevel = 0;
        OtherLevel = 0;

        // TODO, DONT FORGET TO REMOVE BELOW
        DayDatas[1].CreditMoney = 50000;
        DayDatas[2].CreditMoney = 60000;
        DayDatas[3].CreditMoney = 70000;
        DesireMoney = 900000;
    }
    

    

    [System.Serializable]
    public class DayData
    {
        public long StreamingCounter = 0;
        public long CreditMoney = 50000;

        public long GainedSubscriber = 0;
        public long GainedViews = 0;
        public long GainedMoney = 100000;

        public DayData()
        {
            StreamingCounter = 0;
            CreditMoney = 0;

            GainedSubscriber = 0;
            GainedViews = 0;
            // GainedMoney = UnityEngine.Random.Range(10000, 100000);
            GainedMoney = 1000000;
        }
    }

    public void GetChannelInfo(out long TotalSubscriber, out long TotalViews, out long TotalMoney, out long Last3DaysMoney)
    {
        TotalSubscriber = INITIAL_SUBSCRIBER;
        TotalViews = 0;
        TotalMoney = 0;
        for(int i = 0; i <= CurrentDay; i++)
        {
            TotalSubscriber += DayDatas[i].GainedSubscriber;
            TotalViews += DayDatas[i].GainedViews;
            TotalMoney += DayDatas[i].GainedMoney;
        }

        Last3DaysMoney = 0;
        int startDay = CurrentDay - 3;
        if(startDay < 0) startDay = 0;
        for(int i = startDay; i <= CurrentDay; i++)
        {
            Last3DaysMoney += DayDatas[i].GainedMoney;
        }
    }

    public List<long> SubscriberEachDay {get {
        List<long> subs = new List<long>();
        for(int i = 0; i <= CurrentDay; i++)
        {
            subs.Add(DayDatas[i].GainedSubscriber);
        }
        return subs;
    }}
    public List<long> ViewsEachDay {get {
        List<long> views = new List<long>();
        for(int i = 0; i <= CurrentDay; i++)
        {
            views.Add(DayDatas[i].GainedViews);
        }
        return views;
    }}
    public List<long> MoneyEachDay {get {
        List<long> money = new List<long>();
        for(int i = 0; i <= CurrentDay; i++)
        {
            money.Add(DayDatas[i].GainedMoney);
        }
        return money;
    }}


    public long GetSalaryFromToday(){
        int day = CurrentDay;

        if((day+1) % 3 == 1) {
            return DayDatas[day].GainedMoney;
        } else if((day+1) % 3 == 2) {
            return DayDatas[day-1].GainedMoney + DayDatas[day].GainedMoney;
        } else {
            return DayDatas[day-2].GainedMoney + DayDatas[day-1].GainedMoney + DayDatas[day].GainedMoney;
        }
    }

    public List<DayData> DayDatas = new List<DayData>();

    public DayData CurrentDayData => DayDatas[CurrentDay];

    // Story booleans
    public bool HasTalkedToNaoRikiInDay2 = false;

    // After kuliah
    public bool HasDoneKuliah = false;
    public bool JustAfterFirstBelanja = false;
    public bool IsMiauCutsceneDone = false;

    public bool GameEnd = false;



    public const float INTEREST_RATE = 0.55f;
    public void ApplyInterest()
    {
        DebitTabunganMoney = (long)(DebitTabunganMoney * (1 + INTEREST_RATE));
    }

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

    // Lose
    public bool IsPinjol = false;
    public bool IsLose = false;

    // PlayButton
    public const long SILVER_PLAY_BUTTON = 60000;
    public const long GOLD_PLAY_BUTTON = 100000;
    public const long DIAMOND_PLAY_BUTTON = 200000;

    // Streamer Games
    public bool IsBungaShooterBought = false;

    // GG Geming
    public int KursiLevel = 0;
    public int MonitorLevel = 0;
    public int OtherLevel = 0;
}
