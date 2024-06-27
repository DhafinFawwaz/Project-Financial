using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemThrower : MonoBehaviour
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] BelanjaList _belanjaList;
    public void Buy(OptionSession optionSession)
    {
        StartCoroutine(SpawnDelay(optionSession));
    }

    IEnumerator SpawnDelay(OptionSession optionSession)
    {
        Option option = optionSession.GetChoosenOption();
        if(option != null) 
        {
            for(int i = 0; i < option.BuyCount; i++)
            {
                _belanjaList.AddToCart(optionSession.OptionData.ItemData);
                if(PlayerCore.Instance != null) 
                {
                    GameObject item = Instantiate(_itemPrefab,PlayerCore.Instance.transform.position, Quaternion.identity);
                    item.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", optionSession.OptionData.ItemData.Sprite.texture);
                    PlayerCore.Instance.Collect(item.transform);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

    }
}
