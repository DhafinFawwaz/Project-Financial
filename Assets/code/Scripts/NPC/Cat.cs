using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] GameObject[] _catList;
    [SerializeField] Texture2D[] _catSprites;
    void Start()
    {
        int catIndex = Random.Range(0, _catList.Length);
        for(int i = 0; i < _catList.Length; i++) _catList[i].SetActive(false);
        _catList[catIndex].SetActive(true);
        _catList[catIndex].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", _catSprites[Random.Range(0, _catSprites.Length)]);
    }
}
