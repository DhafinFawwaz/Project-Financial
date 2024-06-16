using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class MoneyAnimation : TextAnimation
{

    protected override string getFormattedValue(float value)
    {
        return "Rp."+Mathf.RoundToInt(value).ToString();
    }
}
