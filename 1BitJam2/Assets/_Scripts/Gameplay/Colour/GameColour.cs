using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameColour : Singleton<GameColour>
{
    public Material sobelFilterMaterial;
    public Material enemyLineMaterial;
    private float _colourLerp;
    public float colourLerp;
    public Color currentColour;
    public static Color CurrentColour;
    public Color lerpAColour;
    public Color lerpBColour;

    public static event Action<Color> OnColourUpdated;

    private void OnEnable()
    {
        CurrencyController.OnLootUpdate += ChangeColour;
    }

    private void OnDisable()
    {
        CurrencyController.OnLootUpdate -= ChangeColour;
    }

    void Start()
    {
        ChangeColour(1f);
    }

    void Update()
    {
        if (colourLerp != _colourLerp) ChangeColour(colourLerp);
    }

    void ChangeColour(int currentGold)
    {
        ChangeColour((float)currentGold / CurrencyController.Instance.maxGold);
    }

    void ChangeColour(float lerp)
    {
        _colourLerp = colourLerp = lerp;
        Color.RGBToHSV(lerpAColour,out var h1,out var s1,out var v1);
        Color.RGBToHSV(lerpBColour,out var h2,out var s2,out var v2);
        float hLerp = Mathf.Lerp(h1, h2, _colourLerp);
        CurrentColour = currentColour= Color.HSVToRGB(hLerp, s1, v1);
        
        //CurrentColour = currentColour = Color.Lerp(lerpAColour, lerpBColour, _colourLerp);
        if(sobelFilterMaterial)
            sobelFilterMaterial.SetColor("_Outline_Colour", CurrentColour);
        if(enemyLineMaterial)
            enemyLineMaterial.SetColor("_LineColour", CurrentColour);
        OnColourUpdated?.Invoke(CurrentColour);
    }
}
