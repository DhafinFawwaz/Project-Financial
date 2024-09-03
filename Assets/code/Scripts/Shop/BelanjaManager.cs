using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelanjaManager : MonoBehaviour
{
    [SerializeField] Rak[] _rakList;

    [SerializeField] float _inflasiEvery = 30;
    [SerializeField] AnimationUI _inflasiAnimation;
    [SerializeField] Timer _timer;

    void Start()
    {
        _timer.SetTime(_inflasiEvery);
        _timer.Begin();
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


    public void DiskonBesar()
    {
        
    }
}
