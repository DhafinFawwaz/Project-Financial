using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KuliahController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _happinessText;
    [System.Serializable]
    public class Kesialan
    {
        public GameObject Obj;
        public int Happiness;
        public int Health;
    }
    [SerializeField] Kesialan[] _kesialan;

    int _randomInt;
    void Awake()
    {
        _randomInt = Random.Range(0, _kesialan.Length);
        _kesialan[_randomInt].Obj.SetActive(true);
        _healthText.text = _kesialan[_randomInt].Health.ToString();
        _happinessText.text = _kesialan[_randomInt].Happiness.ToString();
    }
    public void ApplyEffect()
    {
        Save.Data.Health -= _kesialan[_randomInt].Health;
        Save.Data.Happiness -= _kesialan[_randomInt].Happiness;
    }

    public void LoadNextScene()
    {
        
    }
}
