using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildTimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private BuildPhase buildPhase;
    
    // Start is called before the first frame update
    void Awake()
    {
        buildPhase = FindObjectOfType<BuildPhase>();
        timerText = GetComponent<TextMeshProUGUI>();
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
        timerText.faceColor = colour;
    }
    // Update is called once per frame
    void Update()
    {
        int remaining = Mathf.CeilToInt(buildPhase.buildTimeRemaining);
        if (remaining >= 0)
        {
            timerText.text = remaining.ToString("00");
        }
        else if (timerText.text.Length > 0) timerText.text = "";
        

    }
}
