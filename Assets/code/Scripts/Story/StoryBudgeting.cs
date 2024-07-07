using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBudgeting : MonoBehaviour
{
    [SerializeField] GameObject _day2Bg;
    [SerializeField] GameObject _pieChart;
    [SerializeField] GameObject _day3Bg;

    void Awake()
    {
        if(Save.Data.CurrentDay <= 1)
        {
            _day2Bg.SetActive(true);
            _day3Bg.SetActive(false);
            _pieChart.SetActive(false);
        }
        else
        {
            _day2Bg.SetActive(false);
            _day3Bg.SetActive(true);
            _pieChart.SetActive(true);
        }


        Save.Data.HasDoneKuliah = true;
    }
}
