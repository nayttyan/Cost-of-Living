using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text valueText;

    public string statType;

    public PlayerStats stats;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        int value = 0;

        if (statType == "happiness")
            value = stats.happiness;

        if (statType == "health")
            value = stats.health;

        if (statType == "energy")
            value = stats.energy;


        slider.value = value;

        valueText.text = value + "/100";
    }
}