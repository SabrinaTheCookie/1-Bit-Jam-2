using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [Serializable]
    public enum GamePhase { Build, Action}

    [SerializeField] private GamePhase currentPhase;

    public GamePhase GetCurrentPhase() { return currentPhase;}
    
    private ActionPhase _actionPhase;
    private BuildPhase _buildPhase;
    
    public static event Action<GamePhase> OnPhaseChanged;

    private void Awake()
    {
        _actionPhase = GetComponent<ActionPhase>();
        _buildPhase = GetComponent<BuildPhase>();
    }
    
    private void OnDisable()
    {
        BuildPhase.OnBuildPhaseComplete -= NextPhase;
        ActionPhase.OnActionPhaseComplete -= NextPhase;
    }

    private void Start()
    {
        _buildPhase.BeginBuildPhase();
        BuildPhase.OnBuildPhaseComplete += NextPhase;
    }

    public void NextPhase()
    {
        BuildPhase.OnBuildPhaseComplete -= NextPhase;
        ActionPhase.OnActionPhaseComplete -= NextPhase;
        
        //Next phase modulo phase count, Looping around to 0.
        currentPhase = (GamePhase)(((int)currentPhase + 1) % Enum.GetValues(typeof(GamePhase)).Length);

        switch (currentPhase)
        {
            case GamePhase.Build:
                _buildPhase.BeginBuildPhase();
                BuildPhase.OnBuildPhaseComplete += NextPhase;
                break;
            case GamePhase.Action:
                _actionPhase.BeginActionPhase();
                ActionPhase.OnActionPhaseComplete += NextPhase;
                break;
        }
        
        OnPhaseChanged?.Invoke(currentPhase);
    }
}
