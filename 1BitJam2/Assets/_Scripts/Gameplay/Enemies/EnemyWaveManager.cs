using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    /* Manages the waves of enemies and the composition of enemy squads. Talks to BuildPhaseManager when the wave is complete. */

    public BuildPhaseManager buildPhaseManager;

    public GameObject enemyPrefab;
    public EnemyBaseClass[] enemyTypes;
    public Transform enemySpawner;

    public int waveNumber;
    public List<Enemy> enemiesRemaining;

    public int minimumSquadSize = 4;
    public int currentSquadSize;

    public GameObject droppedLootPrefab;



    public void BeginNewWave()
    {
        
        /* Generate a new squad of enemies based on the waveNumber. */
        currentSquadSize = minimumSquadSize += waveNumber;

        for (int i = 0; i < currentSquadSize; i++)
        {
            EnemyBaseClass enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            /* Instantiate a new 'squad' of enemies, following squad composition rules. */
            Enemy enemy = Instantiate(enemyPrefab, enemySpawner.position, enemySpawner.rotation).GetComponent<Enemy>();
            enemy.SetData(enemyType);
            
            enemiesRemaining.Add(enemy);
        }
    }


    public void WaveComplete()
    {
        enemiesRemaining.Clear();

        buildPhaseManager.BeginBuildPhase();
    }



    public void DropLoot(float lootAmount)
    {
        if (lootAmount > 0)
        {
            Loot droppedLoot = Instantiate(droppedLootPrefab, transform.position, transform.rotation).GetComponent<Loot>();
            droppedLoot.lootValue = lootAmount;
        }
    }
}
