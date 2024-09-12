using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BungaShooterChecker : MonoBehaviour
{
    [SerializeField] StreamingManager _streamingManager;
    [SerializeField] ButtonUI _button;
    [SerializeField] int _gameIndex = 1;
    [SerializeField] Image _thumbnail;
    [SerializeField] Sprite _availableSprite;

    void Awake()
    {
        if(Save.Data.IsBungaShooterBought) {
            _thumbnail.sprite = _availableSprite;
            _button.enabled = true;
            _button.OnClick.AddListener(OnGameSelected);
        } else {
            _button.enabled = false;
        }
    }

    public void OnGameSelected()
    {
        _streamingManager.OnGameSelected(_gameIndex);
    }
}
