using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourSetter : MonoBehaviour
{
    public Image image;

    public TextMeshProUGUI text;

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
        if(image)
            image.color = colour;
        if(text)
            text.faceColor = colour;
    }
    
}
