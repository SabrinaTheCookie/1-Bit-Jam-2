using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Floor : MonoBehaviour
{ 
    public int floorNumber;
    public Transform objectHolder;
    public GameObject stairUp;
    public GameObject stairDown;
    public GameObject spawner;
    public Loot lootPile;
    public GameObject lootPilePrefab;
    public List<Enemy> enemiesOnFloor;
    public Transform enemyHolder;
    public Transform towerHolder;
    public Transform lootHolder;
    public List<TowerBase> towersOnFloor = new List<TowerBase>();
    public Grid grid;
    public LineRenderer enemyPathRenderer;
    public FloorManager floorManager;
    public bool lastFloor = false;
    public GameObject clutterPrefab;
    public static event Action FloorNowHasEnemies;
    public static event Action FloorIsNowEmpty;


    private void OnEnable()
    {
        Enemy.OnEnemyChangedFloors += CheckIfEnemyIsThisFloor;
        EnemySpawner.OnEnemySpawned += CheckIfEnemyIsThisFloor;
        Enemy.OnEnemyDefeated += CheckIfEnemyIsThisFloor;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyChangedFloors -= CheckIfEnemyIsThisFloor;
        EnemySpawner.OnEnemySpawned -= CheckIfEnemyIsThisFloor;
        Enemy.OnEnemyDefeated -= CheckIfEnemyIsThisFloor;

    }

    public void SetupFloor(FloorManager manager, int floorNumber, int maxFloors)
    {

        floorManager = manager;
        this.floorNumber = floorNumber;
        gameObject.name = $"Floor{this.floorNumber}";
        //Setup grid
        if (floorNumber == floorManager.Floors.Count - 1)
        {
            lastFloor = true;
        }
        grid = new Grid(lastFloor);
        grid.path.DrawPath(enemyPathRenderer);
        


        
        
        

        if (this.floorNumber == 0)
        {
            //Is first floor, use spawner instead of stair up
            PlaceStairsDown(false);
            PlaceEnemySpawner();
            PlaceClutter();
        }
        else if (this.floorNumber == maxFloors - 1)
        {
            //Is last floor, replace stairs Down with loot
            PlaceStairsUp();
            PlaceLootPile();
        }
        else
        {
            //standard floor, place stairs up and down.
            PlaceStairsDown(true);
            PlaceClutter();
        }

    }


    void PlaceClutter()
    {
        /* Populate the empty space in the floor with random dungeon clutter. */
        (List<Vector3>, List<Vector2>) cellData = grid.PopulateGrid();
        List<Vector3> cellsInWorldSpace = cellData.Item1;
        List<Vector2> cells = cellData.Item2;
        for (int i = 0; i < cells.Count; i++)
        {
            int type = Random.Range(0, 5);

            GameObject clutter = Instantiate(clutterPrefab, cellsInWorldSpace[i], Quaternion.identity, transform.GetChild(3));
            clutter.transform.GetChild(type).gameObject.SetActive(true);
            clutter.transform.localPosition = new Vector3(clutter.transform.localPosition.x, 0.5f, clutter.transform.localPosition.z);

            grid.SetCellOccupant(cells[i], clutter);
        }
    }




    void PlaceStairsDown(bool placeUpwardsToo)
    {
        stairDown = Instantiate(stairDown, objectHolder);

        //Left, Right, Top, Bottom
        int side = Random.Range(0, 4);
        (Vector3, float) posRotPair = GetValidStairLocation(side);
        

        /*stairDown.transform.localPosition = posRotPair.Item1;
        stairDown.transform.Rotate(0,posRotPair.Item2,0);*/

        stairDown.transform.localPosition = Grid.ConvertGridToWorldPosition(grid.path.endPos);
        RotateStairs(stairDown.transform);

        if (placeUpwardsToo) PlaceStairsUp(side);
    }

    void PlaceStairsUp(int pairSide=-1)
    {
        stairUp = Instantiate(stairUp, objectHolder);
        (Vector3, float) posRotPair;
        if (pairSide != -1)
        {
            //Get the opposite side to the pair
            pairSide = pairSide == 0 ? 1 : pairSide == 1 ? 0 : pairSide == 2 ? 3 : 2;
            posRotPair = GetValidStairLocation(pairSide);
        }
        else
        {
            //Not paired, place in random position.
            //Left, Right, Top, Bottom
            int side = Random.Range(0, 4);
            posRotPair = GetValidStairLocation(side);
        }
        /*stairUp.transform.localPosition = posRotPair.Item1;
        stairUp.transform.Rotate(0,posRotPair.Item2,0);*/

        stairUp.transform.localPosition = Grid.ConvertGridToWorldPosition(grid.path.startPos);
        RotateStairs(stairUp.transform);
    }


    void RotateStairs(Transform staircase)
    {
        if (staircase.localPosition.x <= -4.5f)
        {
            staircase.Rotate(0, -90, 0);
        }
        else if (staircase.localPosition.x >= 4.5f)
        {
            staircase.Rotate(0, 90, 0);
        }
        else if (staircase.localPosition.z <= -4.5f)
        {
            staircase.Rotate(0, 180, 0);
        }
        else if (staircase.localPosition.z >= 4.5f)
        {
            staircase.Rotate(0, 0, 0);
        }
    }



    void PlaceLootPile()
    {
        lootPile = Instantiate(lootPilePrefab, objectHolder).GetComponent<Loot>();
        lootPile.Init();
    }

    void PlaceEnemySpawner()
    {
        spawner = Instantiate(spawner, objectHolder);
        spawner.transform.localPosition = Grid.ConvertGridToWorldPosition(grid.path.startPos);
        spawner.GetComponent<EnemySpawner>().Init(this);

    }
    
    public (Vector3, float) GetValidStairLocation(int side)
    {
        Vector3 stairPos = Vector3.zero;
        float rotation = 0;
        //Place opposite side of the pair
        switch (side)
        {
            case 0: //Left
                stairPos.x = -4.5f;
                stairPos.z = Random.Range(-4.5f, 4.5f);
                rotation = -90;
                break;
            case 1: //Right
                stairPos.x = 4.5f;
                stairPos.z = Random.Range(-4.5f, 4.5f);
                rotation = 90;
                break;
            case 2: //Top
                stairPos.x = Random.Range(-4.5f, 4.5f);
                stairPos.z = 4.5f;
                break;
            case 3: //Bottom
                stairPos.x = Random.Range(-4.5f, 4.5f);
                stairPos.z = -4.5f;
                rotation = 180;
                break;
        }

        return (stairPos, rotation);
    }

    public Floor GetNextFloor(bool descending)
    {
        if (lastFloor && descending || floorNumber == 0 &! descending) { return this; }
        /* TO DO: This will throw an error if it's out of range! Throw exception. If at top, leave dungeon with loot - if at bottom, set enemy's advancing to false and steal gold. */
        else if (descending) { return floorManager.Floors[floorNumber + 1]; }
        else { return floorManager.Floors[floorNumber - 1]; }
    }

    void CheckIfEnemyIsThisFloor(Enemy enemy, bool escaped)
    {
        if (enemiesOnFloor.Contains(enemy))
        {
            enemiesOnFloor.Remove(enemy);
            if (enemiesOnFloor.Count == 0)
            {
                FloorIsNowEmpty?.Invoke();
            }
        }
    }
    void CheckIfEnemyIsThisFloor(int floor, Enemy enemy)
    {
        //Same floor
        if (floor == floorNumber)
        {
            if (!enemiesOnFloor.Contains(enemy))
            {
                enemiesOnFloor.Add(enemy);
                if (enemiesOnFloor.Count == 1)
                {
                    FloorNowHasEnemies?.Invoke();
                }
            }
        }
        //not same floor, check if it was on this floor and remove.
        else if (enemiesOnFloor.Contains(enemy))
        {
            enemiesOnFloor.Remove(enemy);
            if (enemiesOnFloor.Count == 0)
            {
                FloorIsNowEmpty?.Invoke();
            }
        }
    }
}
