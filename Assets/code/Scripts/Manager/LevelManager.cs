using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public event System.Action<int> OnSetRandomHighscore;
    public void LoadScene(string sceneName)
    {
        SceneLoader.LoadSceneWithProgressBar(sceneName);
    }

    public void SetRandomHighscore()
    {
        int randomNumber = Random.Range(10, 200);
        OnSetRandomHighscore?.Invoke(randomNumber);
    }
    
}
