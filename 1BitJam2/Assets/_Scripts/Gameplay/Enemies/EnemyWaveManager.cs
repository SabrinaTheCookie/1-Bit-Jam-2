using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    public BuildPhase buildPhase;

    public GameObject enemyPrefab;
    public EnemyBaseClass[] enemyTypes;
    public EnemySpawner enemySpawner;

    public int waveNumber;
    public List<Enemy> enemiesRemaining;

    public int minimumSquadSize = 4;
    public int currentSquadSize;

    public GameObject droppedLootPrefab;

    private void OnEnable()
    {
        ActionPhase.OnActionPhaseStarted += BeginNewWave;
    }
    
    private void OnDisable()
    {
        ActionPhase.OnActionPhaseStarted -= BeginNewWave;
    }

    public void Init()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        BeginNewWave();
    }


    public void BeginNewWave()
    {
        StartCoroutine(NewWave());
        
    }

    IEnumerator NewWave()
    {
        /* Generate a new squad of enemies based on the waveNumber. */
        currentSquadSize = minimumSquadSize += waveNumber;

        for (int i = 0; i < currentSquadSize; i++)
        {
            EnemyBaseClass enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            /* Instantiate a new 'squad' of enemies, following squad composition rules. */
            //Enemy enemy = Instantiate(enemyPrefab, enemySpawner.position, enemySpawner.rotation).GetComponent<Enemy>();
            Enemy enemy = enemySpawner.SpawnEnemy(enemyType);
            //enemy.SetData(enemyType);

            enemiesRemaining.Add(enemy);
            yield return new WaitForSeconds(1);
        }
    }

    public void WaveComplete()
    {
        enemiesRemaining.Clear();

        buildPhase.BeginBuildPhase();
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
