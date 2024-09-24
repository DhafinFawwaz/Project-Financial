using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimePlayButtonPopUp : MonoBehaviour
{
    [SerializeField] TransformAnimation _silver;
    [SerializeField] TransformAnimation _gold;
    [SerializeField] TransformAnimation _diamond;

    void Awake()
    {
        long subs = Save.Data.SubscriberAmount;
        if(subs >= SaveData.SILVER_PLAY_BUTTON && !Save.Data.IsFirstTimeSilver) {
            Save.Data.IsFirstTimeSilver = true;
            _silver.gameObject.SetActive(true);
            _silver.TweenLocalScale();
            this.Invoke(() => {
                _silver.SetEnd(Vector3.zero);
                _silver.TweenLocalScale();
            }, 1.5f);
        } else if(subs >= SaveData.GOLD_PLAY_BUTTON && !Save.Data.IsFirstTimeGold) {
            Save.Data.IsFirstTimeGold = true;
            _gold.gameObject.SetActive(true);
            _gold.TweenLocalScale();
            this.Invoke(() => {
                _gold.SetEnd(Vector3.zero);
                _gold.TweenLocalScale();
            }, 1.5f);
        } else if(subs >= SaveData.DIAMOND_PLAY_BUTTON && !Save.Data.IsFirstTimeDiamond) {
            Save.Data.IsFirstTimeDiamond = true;
            _diamond.gameObject.SetActive(true);
            _diamond.TweenLocalScale();
            this.Invoke(() => {
                _diamond.SetEnd(Vector3.zero);
                _diamond.TweenLocalScale();
            }, 1.5f);
        }
    }
}
