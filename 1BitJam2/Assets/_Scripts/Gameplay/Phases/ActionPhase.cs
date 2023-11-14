using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPhase : MonoBehaviour
{
    public static event Action OnActionPhaseComplete;
    public static event Action OnActionPhaseStarted;

    public void BeginActionPhase()
    {
        OnActionPhaseStarted?.Invoke();
        
        //Replace this with whatever completes the phase, perhaps all enemies dying?
        Invoke(nameof(ActionPhaseComplete), 5);
    }

    void ActionPhaseComplete()
    {
        OnActionPhaseComplete?.Invoke();
    }
}
