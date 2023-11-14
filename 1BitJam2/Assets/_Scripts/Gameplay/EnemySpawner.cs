using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    Floor floor;

    public void Init(Floor _floor)
    {
        floor = _floor;
    }


    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnEnemy(enemyType);
        }
    }*/


    public Enemy SpawnEnemy(EnemyBaseClass enemyType)
    {
        Vector3 spawnPoint = Grid.ConvertGridToWorldPosition(floor.grid.path.startPos);
        GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        enemySpawned.GetComponent<Enemy>().Init(floor, floor.grid.path.startPos, enemyType);
        return enemySpawned.GetComponent<Enemy>();
    }
}
