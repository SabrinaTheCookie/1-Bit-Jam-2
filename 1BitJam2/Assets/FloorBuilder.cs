using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
{
    public GameObject floorPrefab;

    public GameObject BuildFloor()
    {
        GameObject newFloor = Instantiate(floorPrefab, transform);
        return newFloor;
    }

    public List<GameObject> BuildFloor(int count)
    {
        List<GameObject> floors = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject floor = BuildFloor();
            floor.name = $"Floor{i}";
            floors.Add(floor);
        }

        return floors;
    }
    
}
