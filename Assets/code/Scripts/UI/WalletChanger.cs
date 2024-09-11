using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalletChanger : MonoBehaviour
{
    [SerializeField] string[] _keinginanScene; 
    [SerializeField] GameObject _walletKeinginan;
    [SerializeField] GameObject _walletKebutuhan;
    void Awake()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        bool isKeinginanScene = false;
        foreach(string scene in _keinginanScene)
        {
            if(scene == currentSceneName)
            {
                isKeinginanScene = true;
                break;
            }
        }
        _walletKeinginan.SetActive(isKeinginanScene);
        _walletKebutuhan.SetActive(!isKeinginanScene);
    }
}
