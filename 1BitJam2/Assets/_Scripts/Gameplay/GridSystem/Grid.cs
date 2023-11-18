using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    /* Constructs a grid of Vector3 positions, evenly spaced along XZ axis. */

    public Dictionary<Vector2, GameObject> cellPositions = new Dictionary<Vector2, GameObject>();
    private const float GRIDSIZE = 1;
    public const float ENEMY_HEIGHT_OFFSET = 0f;
    public Path path;
    public List<Vector2> bannedCells = new List<Vector2>();

    public Grid(bool lastFloor)
    {
        /* Constructor */
        GenerateGrid();
        path = new Path(lastFloor);
    }


    // Generate grid, then populate with scenery, rather than raycast.

    public void GenerateGrid()
    {
        float zOffset = -1;

        for (int column = -1; column < 12; column++)
        {
            float xOffset = -1;

            for (int row = -1; row < 12; row++)
            {
                Vector2 cell = new Vector2(xOffset, zOffset);
                GameObject occupant = null;

                cellPositions.Add(cell, occupant);
                //Debug.Log(column + " " + row + " " + cell + " " + bannedCells.Count);
                if (column == -1 || column >= 10 || row == -1 || row >= 10) { bannedCells.Add(cell); }
                //Debug.Log(cell);
                xOffset += GRIDSIZE;
            }

            zOffset += GRIDSIZE;
        }
        //Debug.Log("======================");
    }



    public (List<Vector3>, List<Vector2>) PopulateGrid()
    {
        List<Vector2> cells = new List<Vector2>();
        List<Vector3> cellsInWorldSpace = new List<Vector3>();

        foreach (var cell in cellPositions)
        {
            if (path.positions.Contains(cell.Key) || bannedCells.Contains(cell.Key) || GetCellOccupant(cell.Key) != null)
            {
                continue;
            }
            else
            {
                int firstRoll = Random.Range(0, 100);
                if (firstRoll < 50) { continue; }
                else
                {
                    int secondRoll = Random.Range(0, 100);
                    if (secondRoll < 50) { continue; }
                    else
                    {
                        cells.Add(cell.Key);

                        Vector3 cellInWorldSpace = ConvertGridToClutterPosition(cell.Key);
                        cellsInWorldSpace.Add(cellInWorldSpace);
                    }
                }
            }
        }

        return (cellsInWorldSpace, cells);
    }




    public GameObject GetCellOccupant(Vector2 cell)
    {
        return cellPositions[cell];
    }


    public void SetCellOccupant(Vector2 cell, GameObject occupant)
    {
        cellPositions[cell] = occupant;
    }


    public Vector2 FindEnemyNextPosition(Vector2 currentPosition, bool advancing)
    {
        int index = path.positions.IndexOf(currentPosition);
        if (index == -1) { Debug.LogError("Enemy not on a valid path position!"); return Vector2.zero; }

        /* Check if Enemy needs to move up or down a floor then move. */
        Vector2 nextPosition;
        if (advancing) 
        {
            if (index + 1 >= path.positions.Count)
            {
                /* Enemy moved beyond stairs, going down a floor. */
                return currentPosition;
            }
            else { nextPosition = path.positions[index + 1]; }
        }
        else 
        {
            if (index - 1 <= 0)
            {
                /* Enemy moved beyond stairs, going up a floor. */
                return currentPosition;
            }
            else { nextPosition = path.positions[index - 1]; }
        }

        return nextPosition;
    }


    public static Vector3 ConvertGridToWorldPosition(Vector2 gridPosition)
    {
        float x = gridPosition.x - 4.5f;
        float y = gridPosition.y - 4.5f;
        Vector3 worldPosition = new Vector3(x, ENEMY_HEIGHT_OFFSET, y);

        return worldPosition;
    }


    public static Vector3 ConvertGridToClutterPosition(Vector2 gridPosition)
    {
        float x = gridPosition.x - 4.5f;
        float y = gridPosition.y - 4.5f;
        Vector3 worldPosition = new Vector3(x, 0.5f, y);

        return worldPosition;
    }
}
