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
    }
    
    void OnDisable()
    {
        BuildPhase.OnBuildPhaseStarted -= Show;
        BuildPhase.OnBuildPhaseComplete -= Hide;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
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
