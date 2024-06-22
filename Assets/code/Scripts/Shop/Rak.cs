using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Rak : Interactable
{
    [SerializeField] Transform _cameraTarget;
    
    [SerializeField] Item _item;
    bool _isCollected = false;
    void Collect(PlayerCore player) {
        _isCollected = true;
        // Item item = Instantiate(_item, transform.position, transform.rotation);
        // player.Collect(item);
        player.MoveCamera(_cameraTarget.position);
    }
    protected override void OnPlayerInteract() {
        base.OnPlayerInteract();
        if(_isCollected) return;
        Collect(_playerInstance);
    }
}
