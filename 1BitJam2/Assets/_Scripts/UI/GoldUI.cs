using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void OnEnable()
    {
        CurrencyController.OnLootUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        CurrencyController.OnLootUpdate -= UpdateUI;
    }

    void UpdateUI(int newCount)
    {
        goldText.text = $"{newCount}";
    }
}
