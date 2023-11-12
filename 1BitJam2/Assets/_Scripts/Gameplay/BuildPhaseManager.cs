using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
    /* Manages the Build Phase between rounds. Talks to EnemyWaveManager when it's time to begin a new round. */

    public EnemyWaveManager enemyWaveManager;


    public void BeginBuildPhase()
    {
        return;
    }


    public void BuildPhaseComplete()
    {
        enemyWaveManager.BeginNewWave();
    }
}
