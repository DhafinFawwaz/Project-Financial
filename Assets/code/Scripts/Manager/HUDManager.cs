using UnityEngine;
using TMPro;
public class HUDManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _moneyText;
    public TextMeshProUGUI MoneyText{get{return _moneyText;}}

    void Awake()
    {
    }
    public void UpdateScore(long newScore)
    {
        _moneyText.text = "$ " + newScore.ToString();
    }

    public void OnPlayerMoneyUpdated(PlayerCore playerCore){
        UpdateScore(playerCore.Stats.Money);
    }
}
