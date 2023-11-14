using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    /* Constructs a grid of Vector3 positions, evenly spaced along XZ axis. */

    public Dictionary<Vector2, GameObject> cellPositions = new Dictionary<Vector2, GameObject>();
    private const float GRIDSIZE = 1;
    public const float ENEMY_HEIGHT_OFFSET = 1.4f;
    public Path path;


    public Grid()
    {
        /* Constructor */
        GenerateGrid();
        path = new Path();
    }


    // Generate grid, then populate with scenery, rather than raycast.

    public void GenerateGrid()
    {
        float zOffset = 0;

        for (int column = 0; column < 10; column++)
        {
            float xOffset = 0;

            for (int row = 0; row < 10; row++)
            {
                Vector2 cell = new Vector2(xOffset, zOffset);
                GameObject occupant = null;

                cellPositions.Add(cell, occupant);
                xOffset += GRIDSIZE;
            }

            zOffset += GRIDSIZE;
        }
    }



    public void PopulateGrid()
    {

    }




    public GameObject GetCellOccupant(Vector2 cell)
    {
        return cellPositions[cell];
    }


    public void SetCellOccupant(Vector2 cell, GameObject occupant)
    {
        Vector3 previousCell = occupant.transform.position;
        cellPositions[previousCell] = null;
        cellPositions[cell] = occupant;
    }

    public Vector2 FindEnemyNextPosition(Vector2 currentPosition, bool advancing)
    {
        int index = path.positions.IndexOf(currentPosition);
        if (index == -1) { Debug.LogError("Enemy not on a valid path position!"); return Vector2.zero; }

        /* Check if Enemy needs to move up or down a floor then move. */
        Vector2 nextPosition;
        if (advancing) { nextPosition = path.positions[index + 1]; }
        else { nextPosition = path.positions[index - 1]; }
        return nextPosition;
    }


    public static Vector3 ConvertGridToWorldPosition(Vector2 gridPosition)
    {
        float x = gridPosition.x - 4.5f;
        float y = gridPosition.y - 4.5f;
        Vector3 worldPosition = new Vector3(x, ENEMY_HEIGHT_OFFSET, y);

        return worldPosition;
    }
}
