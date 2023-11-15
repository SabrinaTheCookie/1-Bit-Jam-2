using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPhase : MonoBehaviour
{
    public float secondsPerTick;
    public static float SecondsPerTick;
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
        SecondsPerTick = secondsPerTick;
    }

    void Update()
    {
        if (SecondsPerTick != secondsPerTick) SecondsPerTick = secondsPerTick;
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
