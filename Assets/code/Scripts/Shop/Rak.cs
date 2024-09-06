using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
public class Rak : Interactable
{
    public static Action<Rak> s_OnRakInteract;
    [SerializeField] Transform _cameraTarget;
    
    [SerializeField] Item _item;
    public ItemData ItemData => _optionDataCopy.ItemData;
    [SerializeField] OptionData _optionData;
    public OptionData OptionData => _optionDataCopy;

    const float INITIAL_SESSION_TIME = 6;
    public float SessionTime = INITIAL_SESSION_TIME; // will be set by OptionSession


    const float INFLATE_PERCENTAGE = 1.2f;
    public void Inflate()
    {
        _optionDataCopy.Content[0].Price = (int)(_optionDataCopy.Content[0].Price * INFLATE_PERCENTAGE);
        _optionDataCopy.Content[1].Price = (int)(_optionDataCopy.Content[1].Price * INFLATE_PERCENTAGE);
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Price = (int)(_optionDataCopy.Content[2].Price * INFLATE_PERCENTAGE);

        
        _optionDataCopy.Content[0].Inflasi = true;
        _optionDataCopy.Content[1].Inflasi = true;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Inflasi = true;
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
        if(_isLocked) {
            s_OnAccessedWhenLocked?.Invoke();
            return;
        }

        base.OnPlayerInteract();
        if(_isCollected) return;
        Collect(_playerInstance);
    }


    OptionData _optionDataCopy;
    void Awake()
    {
        _optionDataCopy = Instantiate(_optionData);
        ReGacha();
    }

    public void ReGacha()
    {
        _optionDataCopy.Content[0].Inflasi = false;
        _optionDataCopy.Content[1].Inflasi = false;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Inflasi = false;



        // init all
        _optionDataCopy.Content[0].Price = (int)_optionDataCopy.ItemData.Price;
        _optionDataCopy.Content[1].Price = (int)_optionDataCopy.ItemData.Price;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Price = (int)_optionDataCopy.ItemData.Price;

        _optionDataCopy.Content[0].Health = (int)_optionDataCopy.ItemData.Health;
        _optionDataCopy.Content[1].Health = (int)_optionDataCopy.ItemData.Health;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Health = (int)_optionDataCopy.ItemData.Health;

        
        _optionDataCopy.Content[0].Happiness = (int)_optionDataCopy.ItemData.Happiness;
        _optionDataCopy.Content[1].Happiness = (int)_optionDataCopy.ItemData.Happiness;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Happiness = (int)_optionDataCopy.ItemData.Happiness;

        

        int small = UnityEngine.Random.Range(30, 101);
        int medium = UnityEngine.Random.Range(55, 101);
        int large = UnityEngine.Random.Range(80, 101);
        float random = UnityEngine.Random.value;
        if(random < 0.33f) {
            _optionDataCopy.Content[0].Quality = small;
            _optionDataCopy.Content[1].Quality = medium;
            if(_optionDataCopy.Content[2] != null)
                _optionDataCopy.Content[2].Quality = large;
        } else if(random < 0.66f) {
            _optionDataCopy.Content[0].Quality = medium;
            _optionDataCopy.Content[1].Quality = large;
            if(_optionDataCopy.Content[2] != null)
                _optionDataCopy.Content[2].Quality = small;
        } else {
            _optionDataCopy.Content[0].Quality = large;
            _optionDataCopy.Content[1].Quality = small;
            if(_optionDataCopy.Content[2] != null)
                _optionDataCopy.Content[2].Quality = medium;
        }



        // _optionDataCopy.Content[1].Price += 2000;
        // if(_optionDataCopy.Content[2] != null) _optionDataCopy.Content[2].Price -= 2000;

        // Diskon
        // 0
        // if(UnityEngine.Random.value < 0.1f) _optionDataCopy.Content[0].Price = (int)(_optionDataCopy.Content[0].Price * UnityEngine.Random.Range(0.7f, 0.9f));
        // if(UnityEngine.Random.value < 0.4f) _optionDataCopy.Content[1].Price = (int)(_optionDataCopy.Content[1].Price * UnityEngine.Random.Range(0.7f, 0.9f));
        // if(_optionDataCopy.Content[2] != null)
        //     if(UnityEngine.Random.value < 0.4f)
        //         _optionDataCopy.Content[2].Price = (int)(_optionDataCopy.Content[2].Price * UnityEngine.Random.Range(0.7f, 0.9f));

        _optionData.Content[0].Price = (int)(_optionData.Content[0].Price * UnityEngine.Random.Range(0.5f, 1.5f));
        _optionData.Content[1].Price = (int)(_optionData.Content[1].Price * UnityEngine.Random.Range(0.5f, 1.5f));
        if(_optionData.Content[2] != null)
            _optionData.Content[2].Price = (int)(_optionData.Content[2].Price * UnityEngine.Random.Range(0.5f, 1.5f));
    }





    // Discount
    [SerializeField] GameObject _lightenGO;
    public void SetLightenUp(bool lighten)
    {
        _lightenGO.SetActive(lighten);
    }
    const float DISCOUNT_PERCENTAGE = 0.5f;

    public void Discount()
    {
        SetLightenUp(true);

        _optionDataCopy.Content[0].Price = (int)(_optionDataCopy.Content[0].Price * DISCOUNT_PERCENTAGE);
        _optionDataCopy.Content[1].Price = (int)(_optionDataCopy.Content[1].Price * DISCOUNT_PERCENTAGE);
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Price = (int)(_optionDataCopy.Content[2].Price * DISCOUNT_PERCENTAGE);

        
        _optionDataCopy.Content[0].Diskon = true;
        _optionDataCopy.Content[1].Diskon = true;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Diskon = true;
    }
    public void StopDiscount()
    {
        SetLightenUp(false);

        _optionDataCopy.Content[0].Price = (int)(_optionDataCopy.Content[0].Price / DISCOUNT_PERCENTAGE);
        _optionDataCopy.Content[1].Price = (int)(_optionDataCopy.Content[1].Price / DISCOUNT_PERCENTAGE);
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Price = (int)(_optionDataCopy.Content[2].Price / DISCOUNT_PERCENTAGE);

        
        _optionDataCopy.Content[0].Diskon = false;
        _optionDataCopy.Content[1].Diskon = false;
        if(_optionDataCopy.Content[2] != null)
            _optionDataCopy.Content[2].Diskon = false;
    }


    // Locked
    [SerializeField] GameObject _darkenGO;
    bool _isLocked = false;
    public void SetDarken(bool darken)
    {
        _darkenGO.SetActive(darken);

        if(!darken) {
            _isCollected = false;

            if(_isLocked) SessionTime = INITIAL_SESSION_TIME;
        }

        _isLocked = darken;
    }
    public static Action s_OnAccessedWhenLocked;

}
