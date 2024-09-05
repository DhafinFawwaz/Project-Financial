using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPage : MonoBehaviour
{
    int _currentPage = 0;
    void Awake()
    {
        _currentPage = 0;
        SetPage(_currentPage);
    }

    public void NextPage()
    {
        SetPage(_currentPage + 1);
    }

    public void PrevPage()
    {
        SetPage(_currentPage - 1);
    }

    public void SetPage(int page)
    {
        if(page < 0 || page >= transform.childCount) return;
        _currentPage = page;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _currentPage);
        }
    }
}
