using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildPhase : MonoBehaviour
{
    public static event Action OnBuildPhaseComplete; 
    public static event Action OnBuildPhaseStarted;
    
    public float buildPhaseDuration = 5;
    public float buildTimeRemaining;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        if (buildTimeRemaining > 0)
        {
            buildTimeRemaining -= Time.deltaTime;
            if (buildTimeRemaining < 0)
            {
                buildTimeRemaining = -1;
                BuildPhaseComplete();
            }
        }
    }

    public void BeginBuildPhase()
    {
        OnBuildPhaseStarted?.Invoke();
        buildTimeRemaining = buildPhaseDuration;
    }

    public void EndBuildPhase()
    {
        buildTimeRemaining = -1;
        BuildPhaseComplete();
    }
    
    public void BuildPhaseComplete()
    {
        OnBuildPhaseComplete?.Invoke();
    }
}
