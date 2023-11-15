using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraversalTextUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
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
        text.faceColor = colour;
    }
    
}
