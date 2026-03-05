using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text moneyText;

    public PlayerStats stats;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        moneyText.text = stats.money.ToString("N0") + " $";
    }
}