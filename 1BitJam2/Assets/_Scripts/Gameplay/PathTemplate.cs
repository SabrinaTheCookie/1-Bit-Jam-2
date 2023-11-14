using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Path()
    {
        /* Constructor for a random path preset */
        //Debug.Log(Resources.LoadAll("Path Templates").Length);

        PathTemplate[] possiblePaths = Resources.LoadAll<PathTemplate>("Path Templates");
        PathTemplate path = possiblePaths[Random.Range(0, possiblePaths.Length)];
        positions = path.positions;
        startPos = positions[0];
        endPos = positions[positions.Count - 1];
    }


}

