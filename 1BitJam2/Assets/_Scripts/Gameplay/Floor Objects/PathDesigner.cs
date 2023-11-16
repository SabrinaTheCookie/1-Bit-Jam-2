using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathDesigner : MonoBehaviour
{
    public PathTemplate path;

    public bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (!drawGizmos || path == null) { return; }
        
        /* Check for duplicate positions */
        for (int i = 0; i < path.positions.Count; i++)
        {
            Vector2 positionToCompare = path.positions[i];
            for (int j = 0; j < path.positions.Count; j++)
            {
                if (j == i) { continue; }

                if (path.positions[i] == path.positions[j])
                {
                    Debug.LogWarning($"Found duplicate at {path.positions[j]}");
                    Gizmos.color = Color.red;
                }
            }
        }

        /* Check that all xy values are a distance of 1 apart */
        for (int i = 0; i < path.positions.Count - 1; i++)
        {
            Vector3 startPos = new Vector3(path.positions[i].x, 0.2f, path.positions[i].y);
            Vector3 endPos = new Vector3(path.positions[i + 1].x, 0.2f, path.positions[i + 1].y);

            if (Vector3.Distance(startPos, endPos) != 1)
            {
                Debug.LogWarning($"Points {startPos}, {endPos} have an illegal distance! Please set to 1.");
                Gizmos.color = Color.red;
            }
        }




        /* Actually drawing the path */
        for (int i = 0; i < path.positions.Count - 1; i++)
        {
            Vector3 startPos = new Vector3(path.positions[i].x + 0.5f, 0.2f, path.positions[i].y + 0.5f);
            Vector3 endPos = new Vector3(path.positions[i + 1].x + 0.5f, 0.2f, path.positions[i + 1].y + 0.5f);
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawSphere(endPos, 0.1f);
        }

        /* Draw a cube over the start and end positions of the path */
        Gizmos.color = Color.green;
        Vector3 entryPos = new Vector3(path.positions[0].x + 0.5f, 0.2f, path.positions[0].y + 0.5f);
        Gizmos.DrawCube(entryPos, Vector3.one * 0.5f);

        Gizmos.color = Color.red;
        Vector3 exitPos = new Vector3(path.positions[path.positions.Count - 1].x + 0.5f, 0.2f, path.positions[path.positions.Count - 1].y + 0.5f);
        Gizmos.DrawCube(exitPos, Vector3.one * 0.5f);

        /* Draw the bounding box of the grid */
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(0, 0.1f, 0);
        Vector3 bottomRight = new Vector3(10, 0.1f, 0);
        Vector3 topLeft = new Vector3(0, 0.1f, 10);
        Vector3 topRight = new Vector3(10, 0.1f, 10);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
