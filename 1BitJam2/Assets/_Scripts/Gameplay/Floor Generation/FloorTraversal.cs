using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorTraversal : MonoBehaviour
{
    [Header("Components")]
    private FloorManager _manager;
    public static event Action OnTraversalStarted;
    public static event Action OnTraversalEnded;
    
    [Header("Traversal")]
    public int currentFloor;
    public bool isTraversing;
    public int traversalInputDirection;
    private float _traversalDuration;
    public float traversalDuration;

    [Header("Animation")]
    public AnimationCurve translateCurve;
    public Vector3 focusedFloorScale;
    public Vector3 unfocusedFloorScale;

    [Header("Fast Forward")]
    public bool fastForwardActive;
    public float timeHoldingTraversal;
    public float holdTimeForFastTraversal;
    public float fastTraversalSpeedMultiplier;
    
    [Header("Multi-Traversal")]
    private bool isMultiTraversing;
    public float multiTraversalSpeedMultiplier;
    

    private void Awake()
    {
        _manager = GetComponent<FloorManager>();
        _traversalDuration = traversalDuration;

    }

    void OnEnable()
    {
        InputManager.OnTraversePressed += StartTraversal;
        InputManager.OnTraverseReleased += EndTraversal;
    }

    void OnDisable()
    {
        InputManager.OnTraversePressed -= StartTraversal;
        InputManager.OnTraverseReleased -= EndTraversal;
    }

    void Start()
    {
        UpdateFloorScale(_manager.Floors);
    }

    void Update()
    {
        //Only update timer if holding key
        if (traversalInputDirection == 0) return;
        timeHoldingTraversal += Time.deltaTime;
        //If its already fast forwarding, return.
        if (!fastForwardActive & timeHoldingTraversal > holdTimeForFastTraversal)
        {
            fastForwardActive = true;
        }
    }

    void StartTraversal(int input)
    {
        if(input == 0) return;
        

        traversalInputDirection = input;
        Traverse(-input);
    }

    void EndTraversal()
    {
        traversalInputDirection = 0;
        timeHoldingTraversal = 0;
        fastForwardActive = false;
    }
    [ContextMenu("Traverse Downwards Multiple")]
    void TraverseMultipleDown()
    {
        MultiTraversal(1,3);

    }
    [ContextMenu("Traverse Upwards Multiple")]
    void TraverseMultipleUp()
    {
        MultiTraversal(-1,3);
    }

    public void TraverseToFloor(int floor)
    {
        int floorsToMove = floor - currentFloor;
        MultiTraversal(floorsToMove);
    }
    void MultiTraversal(int floorsAndDirection)
    {
        int direction = floorsAndDirection < 0 ? -1 : 1;
        MultiTraversal(direction, Mathf.Abs(floorsAndDirection));
    }

    void MultiTraversal(int direction, int numberOfFloors)
    {
        isMultiTraversing = true;
        Traverse(direction, numberOfFloors);
    }


    private void Traverse(int direction, int numberOfFloors=1)
    {
        if (direction == 1 && currentFloor == _manager.Floors.Count - 1) return; 
        if (direction == -1 && currentFloor == 0) return; 
        if (isTraversing) return;
        isTraversing = true;
        OnTraversalStarted?.Invoke();
        int nextFloor = currentFloor + Mathf.RoundToInt(direction);
        List<Floor> floors = _manager.Floors;
        StartCoroutine(TraverseOverTime(floors, direction, nextFloor, numberOfFloors));
    }

    private void TraversalComplete(int direction, int numberOfFloors)
    {
        if (!isTraversing) return;
        isTraversing = false;
        OnTraversalEnded?.Invoke();
        if (numberOfFloors > 0)
        {
            Traverse(direction, numberOfFloors);
        }
        else if (isMultiTraversing) isMultiTraversing = false;
        if (traversalInputDirection != 0)
        {
            StartTraversal(traversalInputDirection);
        }
    }

    private IEnumerator TraverseOverTime(List<Floor> targets, int traversal, int nextFloor, int floorsToTraverse=1)
    {
        //Generate start and end scale/positions
        List<Vector3> startScales = new List<Vector3>();
        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endScales = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        for (int i = 0; i < targets.Count; i++)
        {
            bool isNextFloor = nextFloor == i;
            float translationDistance = _manager.YSpaceBetweenFloors * (isNextFloor || currentFloor == i ? 1.5f : 1);

            //Start scale/position
            startScales.Add(targets[i].transform.localScale);
            startPositions.Add(targets[i].transform.position);
            //End Scale
            endScales.Add(isNextFloor ? focusedFloorScale : unfocusedFloorScale);
            //Calculate end position
            Vector3 endPos = startPositions[i];
            endPos.y += (translationDistance * traversal);
            endPositions.Add(Vector3Int.FloorToInt(endPos));
        }

        //Iterate and translate/scale all floors
        float t = 0;
        while (t < _traversalDuration)
        {
            float curveT = translateCurve.Evaluate(t / _traversalDuration);
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].transform.localScale = Vector3.Lerp(startScales[i], endScales[i], curveT);
                targets[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], curveT);
            }
            t += Time.deltaTime * (isMultiTraversing ? multiTraversalSpeedMultiplier : (fastForwardActive ? fastTraversalSpeedMultiplier : 1));

            yield return null;
        }

        //Set to end value
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].transform.localScale = endScales[i];
            targets[i].transform.position = endPositions[i];
        }
        currentFloor = nextFloor;
        floorsToTraverse--;
        TraversalComplete(traversal, floorsToTraverse);

        yield return null;
    }

    void UpdateFloorScale(List<Floor> floors)
    {
        for (int i = 0; i < floors.Count; i++)
        {
            floors[i].transform.localScale = (i != currentFloor) ? unfocusedFloorScale : focusedFloorScale;
        }
    }
}
