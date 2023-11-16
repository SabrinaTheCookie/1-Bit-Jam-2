using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourSetter : MonoBehaviour
{
    public Image[] image;

    public TextMeshProUGUI[] text;

    public SpriteRenderer[] spriteRenderer;

    private void OnEnable()
    {
        GameColour.OnColourUpdated += ColourUpdate;
    }

    private void OnDisable()
    {
        GameColour.OnColourUpdated -= ColourUpdate;

    }

    private void Awake()
    {
        ColourUpdate(GameColour.CurrentColor);
    }

    void ColourUpdate(Color colour)
    {
        foreach(Image obj in image)
            obj.color = colour;
        foreach(TextMeshProUGUI obj in text)
            obj.faceColor = colour;
        foreach(SpriteRenderer obj in spriteRenderer)
            obj.color = colour;
    }
    
}
