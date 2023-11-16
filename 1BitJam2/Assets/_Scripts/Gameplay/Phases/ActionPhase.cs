using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionPhase : MonoBehaviour
{
    public float tickMultiplier;
    public static float TickMultiplier;
    public static event Action OnActionPhaseComplete;
    public static event Action OnActionPhaseStarted;

    private void OnEnable()
    {
        EnemyWaveManager.OnWaveComplete += ActionPhaseComplete;
    }

    private void OnDisable()
    {
        EnemyWaveManager.OnWaveComplete -= ActionPhaseComplete;

    }

    void Awake()
    {
        TickMultiplier = tickMultiplier;
    }

    void Update()
    {
        if (TickMultiplier != tickMultiplier) TickMultiplier = tickMultiplier;
    }

    public void BeginActionPhase()
    {
        OnActionPhaseStarted?.Invoke();
    }

    void ActionPhaseComplete()
    {
        OnActionPhaseComplete?.Invoke();
    }
}
