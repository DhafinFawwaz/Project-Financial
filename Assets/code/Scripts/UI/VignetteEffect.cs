using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteEffect : MonoBehaviour
{
    [SerializeField] GraphicsAnimation[] _vignetteLevels;
    int _currentLevel = 0;
    public void IncreaseLevel()
    {
        gameObject.SetActive(true);
        if(_currentLevel < _vignetteLevels.Length - 1)
        {
            _vignetteLevels[_currentLevel].gameObject.SetActive(true);
            _vignetteLevels[_currentLevel].Target.color = new Color(1,1,1,0);
            _vignetteLevels[_currentLevel].SetEndColor(Color.white).Play();
            _currentLevel++;
        }
    }

    public void ResetLevel()
    {
        foreach(GraphicsAnimation ga in _vignetteLevels)
        {
            ga.gameObject.SetActive(false);
        }
    }
    
}
