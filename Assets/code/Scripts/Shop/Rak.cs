using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Rak : Interactable
{
    [SerializeField] Item _item;
    bool _isCollected = false;
    void Collect(PlayerCore player) {
        _isCollected = true;
        Item item = Instantiate(_item, transform.position, transform.rotation);
        player.Collect(item);
    }
    protected override void OnPlayerInteract() {
        base.OnPlayerInteract();
        if(_isCollected) return;
        Collect(_playerInstance);
    }
}
