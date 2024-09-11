using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonActivator : MonoBehaviour
{
    [SerializeField] GameObject _silverButton;
    [SerializeField] GameObject _goldButton;
    [SerializeField] GameObject _diamondButton;
    void Awake()
    {
        _silverButton.SetActive(false);
        _goldButton.SetActive(false);
        _diamondButton.SetActive(false);

        if(Save.Data.SubscriberAmount >= SaveData.SILVER_PLAY_BUTTON) {
            _silverButton.SetActive(true);
        } else if(Save.Data.SubscriberAmount >= SaveData.GOLD_PLAY_BUTTON) {
            _goldButton.SetActive(true);
        } else if(Save.Data.SubscriberAmount >= SaveData.DIAMOND_PLAY_BUTTON) {
            _diamondButton.SetActive(true);
        }
    }
}
