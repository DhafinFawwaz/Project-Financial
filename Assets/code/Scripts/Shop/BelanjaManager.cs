using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelanjaManager : MonoBehaviour
{
    [SerializeField] Rak[] _rakList;

    [SerializeField] float INFLASI_EVERY = 180;
    [SerializeField] AnimationUI _inflasiAnimation;
    [SerializeField] Timer _timer;

    void Start()
    {
        _timer.SetTime(INFLASI_EVERY);
        _timer.Begin();
    }


    public void Inflasi()
    {
        _timer.SetTime(INFLASI_EVERY);
        _timer.Begin();
        _inflasiAnimation.Play();
        foreach(var rak in _rakList)
        {
            rak.Inflate();
        }
    }
}
