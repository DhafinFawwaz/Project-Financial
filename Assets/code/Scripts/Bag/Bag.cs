using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
    [SerializeField] AnimationUI _animationUI;
    [SerializeField] AnimationUI _animationUIReversed;
    [SerializeField] Gosokable[] _gosokables;
    [SerializeField] GameObject[] _gosokableBlokers;
    [SerializeField] float _bagAnimationDuration = 0.3f;
    bool _isShowing = false;
    [SerializeField] PopUp _popUpBuy;
    [SerializeField] PopUp _popUpCannotBuy;
    [SerializeField] PopUp _popUpInsufficient;
    [SerializeField] TextMeshProUGUI _priceText;


    float _lastTime = 0;

    // Called by hovering over the bag at the bottom right of the screen
    public void Toggle()
    {
        if(Time.time - _lastTime < _bagAnimationDuration) return;
        _lastTime = Time.time;

        _isShowing = !_isShowing;
        if(_isShowing) _animationUI.Play();
        else _animationUIReversed.Play();
    }

    void Update()
    {
        if(_popUpBuy.IsVisible || _popUpCannotBuy.IsVisible) return;

        if(InputManager.GetKeyDown(KeyCode.R))
        {
            Toggle();
        }


        foreach(Gosokable gosokable in _gosokables)
        {
            if(gosokable.gameObject.activeInHierarchy && !_gosokableBlokers[gosokable.ItemID].activeInHierarchy && gosokable.ContainsCursor())
            {
                if(gosokable.IsAllClear())
                {
                    gosokable.gameObject.SetActive(false);
                } 
                else 
                {
                    MouseCursor.Main.SetToGosok();
                    if(Input.GetMouseButton(0))
                    {
                        gosokable.Gosok(Input.mousePosition);
                    }
                    return;
                }
            }
        }
        MouseCursor.Main.SetToCursor();
    }


    int _selectedItemID = -1;
    public void Buy(int id)
    {
        if(_popUpBuy.IsVisible || _popUpCannotBuy.IsVisible) return;
        MouseCursor.Main.SetToCursor();
        if(IsBuyable(id))
        {
            _priceText.text = "3";
            _popUpBuy.Show();
            _selectedItemID = id;
        }
        else
        {
            _popUpCannotBuy.Show();
        }
    }

    // if id is even, true
    // if id is odd and the left item is bought, true
    bool IsBuyable(int id)
    {
        if(id % 2 == 0) return true;
        if(isBought(id - 1)) return true;
        return false;
    }

    bool isBought(int id)
    {
        return !_gosokableBlokers[id].activeInHierarchy;
    }


    [SerializeField] int[] _prices;
    [SerializeField] UnityEvent _onBuyConfirmed;
    public void BuyConfirmed()
    {
        _popUpBuy.Hide();
        _gosokableBlokers[_selectedItemID].SetActive(false);

        Save.Data.SkillPoints -= _prices[_selectedItemID];
        _onBuyConfirmed?.Invoke();
    }
}
