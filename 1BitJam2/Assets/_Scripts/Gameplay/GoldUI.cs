using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    private Image image;

    private TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameColour.OnColourUpdated += ColourUpdate;
    }

    private void OnDisable()
    {
        GameColour.OnColourUpdated -= ColourUpdate;

    }

    void ColourUpdate(Color colour)
    {
        image.color = colour;
        text.faceColor = colour;
    }
    
}
