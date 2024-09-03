using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Rak : Interactable
{
    public static Action<Rak> s_OnRakInteract;
    [SerializeField] Transform _cameraTarget;
    
    [SerializeField] Item _item;
    [SerializeField] OptionData _optionData;
    public OptionData OptionData => _optionData;


    const float INFLATE_PERCENTAGE = 1.2f;
    public void Inflate()
    {
        _optionData.Content[0].Price = (int)(_optionData.Content[0].Price * INFLATE_PERCENTAGE);
        _optionData.Content[1].Price = (int)(_optionData.Content[1].Price * INFLATE_PERCENTAGE);
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Price = (int)(_optionData.Content[2].Price * INFLATE_PERCENTAGE);

        
        _optionData.Content[0].Inflasi = true;
        _optionData.Content[1].Inflasi = true;
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Inflasi = true;
    }

    bool _isCollected = false;
    public bool IsCollected {set => _isCollected = value;}
    [HideInInspector] public PlayerCore Player = null;
    void Collect(PlayerCore player) {
        _isCollected = true;
        // Item item = Instantiate(_item, transform.position, transform.rotation);
        // player.Collect(item);
        Player = player;
        player.MoveCamera(_cameraTarget.position);
        s_OnRakInteract?.Invoke(this);
    }
    protected override void OnPlayerInteract() {
        base.OnPlayerInteract();
        if(_isCollected) return;
        Collect(_playerInstance);
    }


    void Awake()
    {
        _optionData = Instantiate(_optionData);


        _optionData.Content[0].Inflasi = false;
        _optionData.Content[1].Inflasi = false;
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Inflasi = false;



        // init all
        _optionData.Content[0].Price = (int)_optionData.ItemData.Price;
        _optionData.Content[1].Price = (int)_optionData.ItemData.Price;
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Price = (int)_optionData.ItemData.Price;

        _optionData.Content[0].Health = (int)_optionData.ItemData.Health;
        _optionData.Content[1].Health = (int)_optionData.ItemData.Health;
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Health = (int)_optionData.ItemData.Health;

        
        _optionData.Content[0].Happiness = (int)_optionData.ItemData.Happiness;
        _optionData.Content[1].Happiness = (int)_optionData.ItemData.Happiness;
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Happiness = (int)_optionData.ItemData.Happiness;

        



        _optionData.Content[1].Price += 2000;
        if(_optionData.Content[2] != null) _optionData.Content[2].Price -= 2000;

        // Diskon
        // 0
        if(UnityEngine.Random.value < 0.1f) _optionData.Content[0].Price = (int)(_optionData.Content[0].Price * UnityEngine.Random.Range(0.7f, 0.9f));
        if(UnityEngine.Random.value < 0.4f) _optionData.Content[1].Price = (int)(_optionData.Content[1].Price * UnityEngine.Random.Range(0.7f, 0.9f));
        if(_optionData.Content[2] != null)
            if(UnityEngine.Random.value < 0.4f)
                _optionData.Content[2].Price = (int)(_optionData.Content[2].Price * UnityEngine.Random.Range(0.7f, 0.9f));

    }





    [SerializeField] GameObject _lightenGO;
    public void SetLightenUp(bool lighten)
    {
        _lightenGO.SetActive(lighten);
    }
}
