using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelanjaKeranjang : MonoBehaviour
{
    [SerializeField] OptionSession _optionSession;

    void OnEnable()
    {
        _optionSession.OnOptionChoosen.AddListener(OnOptionChoosen);
    }

    void OnDisable()
    {
        _optionSession.OnOptionChoosen.RemoveListener(OnOptionChoosen);
    }
    
    int _totalPercentage = 0;
    int _totalItems = 0;
    void OnOptionChoosen(OptionSession optionSession)
    {
        var option = optionSession.GetChoosenOption();
        // _totalPercentage += option.Content.Quality * option.BuyCount;
        // _totalItems += option.BuyCount;

        // AfterBelanja.SetData(_totalItems, _totalPercentage);
    }
}
