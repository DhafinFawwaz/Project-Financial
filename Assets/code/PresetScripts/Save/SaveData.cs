using System.Collections.Generic;
using Unity.VisualScripting;
[System.Serializable]
public class SaveData
{
    const int MAX_DAY = 15;
    public long Money = 100000;
    public double Happiness = 100;
    public double Health = 100;
    public int SkillPoin = 0;
    public int CurrentDay = 1;
    public long SubscriberAmount = 50000;

    public List<ItemCount> CurrentListBelanja = new List<ItemCount>();
    public long CurrentBelanjaMoney = 0;
    
    public SaveData()
    {
        Money = 100000;
        Happiness = 100;
        Health = 100;
        CurrentDay = 1;
        CurrentListBelanja = new List<ItemCount>();
        CurrentBelanjaMoney = 0;
        SkillPoin = 0;
        SubscriberAmount = 50000;

        for(int i = 0; i < MAX_DAY; i++)
        {
            StreamingCounter.Add(0);
            GainedSubscriberEachDay.Add(0);
        }
    }



    // Story booleans
    public List<long> StreamingCounter = new List<long>();
    public List<long> GainedSubscriberEachDay = new List<long>();
    public bool HasDay1Sleep = false;
    public bool HasTalkedToNaoRikiInDay2 = false;

}
