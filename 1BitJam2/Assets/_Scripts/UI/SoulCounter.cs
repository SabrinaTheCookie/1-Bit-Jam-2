using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    void OnEnable()
    {
        CurrencyController.OnSoulUpdate += CounterChanged;
    }

    void OnDisable()
    {
        CurrencyController.OnSoulUpdate -= CounterChanged;
    }

    private void Start()
    {
        CounterChanged(CurrencyController.Instance.currentSouls);
    }

    void CounterChanged(int soulCount)
    {
        text.text = $"{soulCount}";
    }
}
