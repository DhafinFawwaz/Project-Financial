using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteEffect : MonoBehaviour
{
    [SerializeField] GraphicsAnimation[] _vignetteLevels;
    int _currentLevel = -1;
    void Awake()
    {
        _currentLevel = -1;
    }

    public void IncreaseLevel()
    {
        gameObject.SetActive(true);
        if(_currentLevel < _vignetteLevels.Length)
        {
            _vignetteLevels[_currentLevel].gameObject.SetActive(true);
            _vignetteLevels[_currentLevel].Target.color = new Color(1,1,1,0);
            _vignetteLevels[_currentLevel].SetEndColor(Color.white).Play();
            _currentLevel++;
        }
    }

    public void SetLevel(int level)
    {
        if(_currentLevel == level) return;
        if(level < 0 || level >= _vignetteLevels.Length) return;
        _currentLevel = level;
        for(int i = 0; i < _currentLevel+1; i++)
        {
            _vignetteLevels[i].gameObject.SetActive(true);
            _vignetteLevels[i].SetEndColor(Color.white).Play();
        }
        for(int i = _currentLevel+1; i < _vignetteLevels.Length; i++)
        {
            _vignetteLevels[i].SetEndColor(new Color(1,1,1,0)).Play();
        }
    }


    public void ResetLevel()
    {
        foreach(GraphicsAnimation ga in _vignetteLevels)
        {
            ga.SetEndColor(new Color(1,1,1,0)).Play();
        }
        _currentLevel = -1;
    }
    
}
