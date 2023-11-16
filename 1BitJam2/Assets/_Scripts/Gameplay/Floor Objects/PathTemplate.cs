using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Path Template", menuName = "ScriptableObjects/Paths", order = 2)]
public class PathTemplate : ScriptableObject
{
    public List<Vector2> positions;
}

public class Path
{
    public List<Vector2> positions;
    public Vector2 startPos;
    public Vector2 endPos;

    public Path(bool lastFloor)
    {
        /* Constructor for a random path preset */
        //Debug.Log(Resources.LoadAll("Path Templates").Length);

        PathTemplate path;

        if (!lastFloor)
        {
            PathTemplate[] possiblePaths = Resources.LoadAll<PathTemplate>("Path Templates");
            //Debug.Log(possiblePaths.Length);
            path = possiblePaths[Random.Range(0, possiblePaths.Length)];
            while (path.name == "BottomPath") { path = possiblePaths[Random.Range(0, possiblePaths.Length)]; }
        }
        else
        {
            path = Resources.Load<PathTemplate>("Path Templates/BottomPath");
        }
        positions = path.positions;
        startPos = positions[0];
        endPos = positions[positions.Count - 1];
    }

    public void DrawPath(LineRenderer enemyPath)
    {
        enemyPath.positionCount = positions.Count;

        Vector3[] linePositions = new Vector3[positions.Count];
        int i = 0;
        foreach (var position in positions)
        {
            Vector3 newPos = new Vector3(position.x - 4.5f, position.y - 4.5f, 0);
            linePositions[i] = newPos;
            i++;
        }

        enemyPath.SetPositions(linePositions);
    }
}

