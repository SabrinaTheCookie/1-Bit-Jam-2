using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    Floor floor;
    public static event Action<int, Enemy> OnEnemySpawned;

    public void Init(Floor _floor)
    {
        floor = _floor;
    }



    public Enemy SpawnEnemy(EnemyBaseClass enemyType)
    {
        Vector3 spawnPoint = Grid.ConvertGridToWorldPosition(floor.grid.path.startPos);
        Enemy enemySpawned = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity).GetComponent<Enemy>();
        enemySpawned.Init(floor, floor.grid.path.startPos, enemyType);
        OnEnemySpawned?.Invoke(floor.floorNumber, enemySpawned);
        return enemySpawned;
    }
}
