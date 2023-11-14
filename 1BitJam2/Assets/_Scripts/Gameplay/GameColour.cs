using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColour : Singleton<GameColour>
{
    public Material sobelFilterMaterial;
    public Material enemyLineMaterial;
    private float _colourLerp;
    public float colourLerp;
    public Color CurrentColor;
    public Color lerpAColour;
    public Color lerpBColour;

    public static event Action<Color> OnColourUpdated;
    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        ChangeColour(colourLerp);
    }

    void Update()
    {
        if (colourLerp != _colourLerp) ChangeColour(colourLerp);
    }

    void ChangeColour(float lerp)
    {
        _colourLerp = lerp;
        CurrentColor = Color.Lerp(lerpAColour, lerpBColour, _colourLerp);
        OnColourUpdated?.Invoke(CurrentColor);
        if(sobelFilterMaterial)
            sobelFilterMaterial.SetColor("_Outline_Colour", CurrentColor);
        if(enemyLineMaterial)
            enemyLineMaterial.SetColor("_LineColour", CurrentColor);
    }
}
