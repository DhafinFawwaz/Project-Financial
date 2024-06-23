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
}
