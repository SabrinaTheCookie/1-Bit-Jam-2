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
    public List<EnemyBaseClass> enemyTypes;
    public EnemySpawner enemySpawner;

    [Min(1)]
    public int waveNumber = 1;
    public List<Enemy> enemiesRemaining;

    [FormerlySerializedAs("minimumSquadSize")] public int startingSquadSize = 4;
    public int currentSquadSize;
    public float timeBetweenSpawns;
    private bool finishedSpawning;

    public EnemyBaseClass pitySouls;

    private bool hasInit;

    public static event Action OnWaveComplete;

    public GameObject fastFowardButton;
    bool fastForwarding;
    bool pitySoulsCollected;

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
        pitySoulsCollected = false;
        finishedSpawning = false;
        /* Generate a new squad of enemies based on the waveNumber. */
        currentSquadSize = startingSquadSize + (waveNumber - 1);

        

        //Can only spawn 1 boss!
        bool hasBoss = false;
        for (int i = 0; i < currentSquadSize; i++)
        {
            EnemyBaseClass enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];

            //Just keep randomizing until you get an unlocked enemy type and not a second boss
            while (enemyType.waveNumberToUnlock > waveNumber || (hasBoss && enemyType.enemyClass == "Boss"))
            {
                enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
                if (enemyType.enemyClass == "Boss") hasBoss = true;
            }

            /* Instantiate a new 'squad' of enemies, following squad composition rules. */
            //Enemy enemy = Instantiate(enemyPrefab, enemySpawner.position, enemySpawner.rotation).GetComponent<Enemy>();
            Enemy enemy = enemySpawner.SpawnEnemy(enemyType);
            //enemy.SetData(enemyType);

            enemiesRemaining.Add(enemy);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        

        finishedSpawning = true;
        fastFowardButton.SetActive(true);
    }


    void EnemyDefeated(Enemy enemyDefeated, bool escaped = false)
    {
        enemiesRemaining.Remove(enemyDefeated);
        
        if(enemiesRemaining.Count == 0 && finishedSpawning) WaveComplete();
    }


    public void WaveComplete()
    {
        if (!pitySoulsCollected)
        {
            pitySoulsCollected = true;

            /* Spawns an invisible pity enemy that will die at the end of the round and grant the player a single soul. Cannot be killed by normal means.
             * This prevents the player from getting stuck in a loop where they can't build anything and can't kill anything either. */
            Enemy pityEnemy = enemySpawner.SpawnPityEnemy(pitySouls);
            enemiesRemaining.Add(pityEnemy);
            return;
        }
        else
        {
            fastFowardButton.SetActive(false);
            fastForwarding = false;
            Time.timeScale = 1.0f;

            waveNumber++;
            OnWaveComplete?.Invoke();
        }
    }


    public void FastForward()
    {
        if (!fastForwarding)
        {
            fastFowardButton.SetActive(false);
            fastForwarding = true;
            Time.timeScale = 3.0f;
        }
    }
}
