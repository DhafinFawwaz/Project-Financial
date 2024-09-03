using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelanjaManager : MonoBehaviour
{
    [SerializeField] Rak[] _rakList;

    [SerializeField] float _inflasiEvery = 30;
    [SerializeField] AnimationUI _inflasiAnimation;
    [SerializeField] Timer _timer;

    
    [Header("Diskon Besar")]
    public float _diskonBesarMin = 35;
    public float _diskonBesarMax = 55;
    [SerializeField] BelanjaList _belanjaList;

    [SerializeField] AnimationUI _diskonBesarAnimation;
    [SerializeField] Timer _timerDiscount;
    [SerializeField] float _discountDuration = 10;
    void Start()
    {
        _timer.SetTime(_inflasiEvery);
        _timer.Begin();


        float diskonTime = Random.Range(_diskonBesarMin, _diskonBesarMax);
        this.Invoke(() => {
            DiskonBesar();
        }, diskonTime);
        Debug.Log(diskonTime);
    }


    public void Inflasi()
    {
        _timer.SetTime(_inflasiEvery);
        _timer.Begin();
        _inflasiAnimation.Play();
        foreach(var rak in _rakList)
        {
            rak.Inflate();
        }
    }


    [SerializeField] int _maxRakToGetDiscount = 3;
    public void DiskonBesar()
    {
        var items = _belanjaList.GetNotInCartItems();
        if(items.Count == 0) return;
        
        List<Rak> targetRaks = new List<Rak>();
        foreach(var rak in _rakList)
        {
            if(items.Exists(x => x.Name == rak.ItemData.Name))
            {
                targetRaks.Add(rak);
            }
        }

        var resultRaks = PickRandom(targetRaks, _maxRakToGetDiscount);        
        _diskonBesarAnimation.Play();

        foreach(var rak in resultRaks)
        {
            rak.Discount();
        }
        _timerDiscount.SetTime(_discountDuration);
        _timerDiscount.Begin();
    }

    public void StopDiskonBesar()
    {
        foreach(var rak in _rakList)
        {
            rak.StopDiscount();
        }
    }

    // must unique
    List<Rak> PickRandom(List<Rak> raks, int amount)
    {
        List<Rak> result = new List<Rak>();
        for(int i = 0; i < amount; i++)
        {
            int randomInt = Random.Range(0, raks.Count);
            result.Add(raks[randomInt]);
            raks.RemoveAt(randomInt);
        }
        return result;
    }
}
