using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
{
    public GameObject floorPrefab;

    public Floor BuildFloor()
    {
        Floor newFloor = Instantiate(floorPrefab, transform).GetComponent<Floor>();
        return newFloor;
    }

    public List<Floor> BuildFloor(int count)
    {
        List<Floor> floors = new List<Floor>();
        for (int i = 0; i < count; i++)
        {
            Floor floor = BuildFloor();
            floors.Add(floor);
        }

        return floors;
    }
    
}
