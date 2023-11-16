using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    public EnemyBaseClass[] enemyTypes;
    public EnemySpawner enemySpawner;

    public int waveNumber;
    public List<Enemy> lightEnemiesRemaining;
    public List<Enemy> mediumEnemiesRemaining;
    public List<Enemy> heavyEnemiesRemaining;
    public List<Enemy> enemiesRemaining;

    public int minimumSquadSize = 4;
    public int currentSquadSize;
    public int numberOfSwapPositionsAllowed = 4;

    public GameObject droppedLootPrefab;

    private bool hasInit;

    public static event Action OnWaveComplete;
    private void OnEnable()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        ActionPhase.OnActionPhaseStarted += BeginNewWave;
        Enemy.OnEnemyDefeated += EnemyDefeated;
    }
    
    private void OnDisable()
    {
        ActionPhase.OnActionPhaseStarted -= BeginNewWave;
        Enemy.OnEnemyDefeated -= EnemyDefeated;
    }


    public void BeginNewWave()
    {
        if (!enemySpawner) enemySpawner = FindObjectOfType<EnemySpawner>();
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

            if (enemyType == enemyTypes[0]) { lightEnemiesRemaining.Add(enemy); }
            else if (enemyType == enemyTypes[1]) { mediumEnemiesRemaining.Add(enemy); }
            else { heavyEnemiesRemaining.Add(enemy); }

            enemiesRemaining.Add(enemy);
            yield return new WaitForSeconds(ActionPhase.SecondsPerTick);
        }

        StartCoroutine(LightEnemyTick());
        StartCoroutine(MediumEnemyTick());
        StartCoroutine(HeavyEnemyTick());
    }


    public enum TurnProgress 
    {
        Incomplete, Repeat, Complete
    }



    IEnumerator LightEnemyTick() 
    {
        while (lightEnemiesRemaining.Count > 0)
        {
            yield return new WaitForSeconds(enemyTypes[0].baseTickRate);

            foreach (Enemy enemy in enemiesRemaining)
            {
                if (enemy.baseTickRate == enemyTypes[0].baseTickRate)
                {
                    TurnProgress turnProgress = enemy.AttemptToMove();
                    while (turnProgress == TurnProgress.Incomplete)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    if (turnProgress == TurnProgress.Repeat)
                    {
                        for (int i = 0; i < numberOfSwapPositionsAllowed; i++)
                        {
                            if (turnProgress == TurnProgress.Repeat) 
                            {
                                turnProgress = enemy.AttemptToMove();
                                while (turnProgress == TurnProgress.Incomplete)
                                {
                                    yield return new WaitForEndOfFrame();
                                }
                            }
                        }
                    }
                }
            }
        }

        StopCoroutine(LightEnemyTick());
    }


    IEnumerator MediumEnemyTick() 
    {
        while (mediumEnemiesRemaining.Count > 0)
        {
            yield return new WaitForSeconds(enemyTypes[1].baseTickRate);

            foreach (Enemy enemy in enemiesRemaining)
            {
                if (enemy.baseTickRate == enemyTypes[1].baseTickRate) 
                {
                    TurnProgress turnProgress = enemy.AttemptToMove();
                    while (turnProgress == TurnProgress.Incomplete)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    if (turnProgress == TurnProgress.Repeat)
                    {
                        for (int i = 0; i < numberOfSwapPositionsAllowed; i++)
                        {
                            if (turnProgress == TurnProgress.Repeat) 
                            {
                                turnProgress = enemy.AttemptToMove();
                                while (turnProgress == TurnProgress.Incomplete)
                                {
                                    yield return new WaitForEndOfFrame();
                                }
                            }
                        }
                    }
                }
            }
        }

        StopCoroutine(MediumEnemyTick());
    }


    IEnumerator HeavyEnemyTick() 
    {
        while (mediumEnemiesRemaining.Count > 0)
        {
            yield return new WaitForSeconds(enemyTypes[2].baseTickRate);

            foreach (Enemy enemy in enemiesRemaining)
            {
                if (enemy.baseTickRate == enemyTypes[2].baseTickRate)
                {
                    TurnProgress turnProgress = enemy.AttemptToMove();
                    while (turnProgress == TurnProgress.Incomplete)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    if (turnProgress == TurnProgress.Repeat)
                    {
                        for (int i = 0; i < numberOfSwapPositionsAllowed; i++)
                        {
                            if (turnProgress == TurnProgress.Repeat) 
                            {
                                turnProgress = enemy.AttemptToMove();
                                while (turnProgress == TurnProgress.Incomplete)
                                {
                                    yield return new WaitForEndOfFrame();
                                }
                            }
                        }
                    }
                }
            }
        }

        StopCoroutine(HeavyEnemyTick()); 
    }
    


    void EnemyDefeated(Enemy enemyDefeated)
    {
        if (lightEnemiesRemaining.Contains(enemyDefeated)) { lightEnemiesRemaining.Remove(enemyDefeated); }
        else if (mediumEnemiesRemaining.Contains(enemyDefeated)) { mediumEnemiesRemaining.Remove(enemyDefeated); }
        else { heavyEnemiesRemaining.Remove(enemyDefeated); }

        enemiesRemaining.Remove(enemyDefeated);
        
        if(enemiesRemaining.Count == 0) WaveComplete();
    }
    public void WaveComplete()
    {
        lightEnemiesRemaining.Clear();
        mediumEnemiesRemaining.Clear();
        heavyEnemiesRemaining.Clear();
        OnWaveComplete?.Invoke();
    }



    public void DropLoot(int lootAmount)
    {
        if (lootAmount > 0)
        {
            Loot droppedLoot = Instantiate(droppedLootPrefab, transform.position, transform.rotation).GetComponent<Loot>();
            droppedLoot.lootValue = lootAmount;
        }
    }
}
