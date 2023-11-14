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

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void BeginBuildPhase()
    {
        OnBuildPhaseStarted?.Invoke();
        Invoke(nameof(BuildPhaseComplete), buildPhaseDuration);
    }


    public void BuildPhaseComplete()
    {
        OnBuildPhaseComplete?.Invoke();
    }
}
