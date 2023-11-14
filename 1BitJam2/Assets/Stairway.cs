using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairway : MonoBehaviour
{
    [Serializable]
    public enum StairwayDirection { Upwards, Downwards }

    public StairwayDirection stairwayDirection;
}
