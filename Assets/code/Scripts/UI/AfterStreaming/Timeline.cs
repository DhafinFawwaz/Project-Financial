using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _dayTexts;
    [SerializeField] GameObject[] _dayObj;
    [SerializeField] GameObject[] _moneyIcons;
    [SerializeField] float _salaryDayOffsetY = 100;

    void Awake()
    {
        int dayPlus1 = Save.Data.CurrentDay+1;
        for(int i = 0; i < 5; i++) {
            _dayObj[i] = transform.GetChild(i).GetChild(1).gameObject;
            _dayTexts[i] = transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            _moneyIcons[i] = transform.GetChild(i).GetChild(2).gameObject;
            _dayObj[i].SetActive(false);
            _moneyIcons[i].SetActive(false);


            if((dayPlus1+i-1) % 3 == 0) {
                _dayObj[i].SetActive(true);
                _moneyIcons[i].SetActive(true);
                _dayTexts[i].text = "Day "+(dayPlus1+i-1).ToString();
            }

        }
        
        _dayObj[0].SetActive(false);
        _moneyIcons[0].SetActive(false);
        _dayObj[1].SetActive(true);
        _moneyIcons[1].SetActive(false);
        _dayTexts[1].text = "Day "+(dayPlus1).ToString();

        if(dayPlus1 % 3 == 0) {
            _moneyIcons[1].SetActive(true);
            // Vector3 pos = _moneyIcons[1].transform.localPosition;
            // pos.y += _salaryDayOffsetY;
            // _moneyIcons[1].transform.localPosition = pos;
        }
    }
}
