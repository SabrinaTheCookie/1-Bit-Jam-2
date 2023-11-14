using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartActionPhaseUI : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI text;
    
    void OnEnable()
    {
        BuildPhase.OnBuildPhaseStarted += Show;
        BuildPhase.OnBuildPhaseComplete += Hide;
        GameColour.OnColourUpdated += ColourUpdate;
    }
    
    void OnDisable()
    {
        BuildPhase.OnBuildPhaseStarted -= Show;
        BuildPhase.OnBuildPhaseComplete -= Hide;
        GameColour.OnColourUpdated -= ColourUpdate;

    }

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void ColourUpdate(Color colour)
    {
        image.color = colour;
    }

    void Show()
    {
        image.enabled = true;
        text.enabled = true;
    }

    void Hide()
    {
        image.enabled = false;
        text.enabled = false;
    }

}
