using System.Collections.Generic;
[System.Serializable]
public class SaveData
{
    public long Money = 100000;
    public double Happiness = 100;
    public double Health = 100;
    public int SkillPoin = 0;
    public int CurrentDay = 1;

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
    }

}
