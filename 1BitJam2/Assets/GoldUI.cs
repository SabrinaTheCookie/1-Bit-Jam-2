using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void OnEnable()
    {
        GameLootController.OnLootUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        GameLootController.OnLootUpdate -= UpdateUI;
    }

    void UpdateUI(int newCount)
    {
        goldText.text = $"{newCount}";
    }
}
