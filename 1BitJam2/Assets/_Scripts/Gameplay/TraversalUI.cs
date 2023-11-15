using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraversalUI : MonoBehaviour
{
    
    private FloorManager _manager;

    public GameObject buttonPrefab;

    private List<Button> _buttons = new List<Button>();

    private float activeScale = 1.5f;

    private void OnEnable()
    {
        FloorTraversal.OnTraversal += UpdateButtons;
    }

    private void OnDisable()
    {
        FloorTraversal.OnTraversal -= UpdateButtons;
    }

    private void Start()
    {
        GenerateButtons();
    }

    void Awake()
    {
        _manager = FindObjectOfType<FloorManager>();
    }

    void GenerateButtons()
    {
        int numberOfFloors = _manager.Floors.Count;
        for(int i = 0; i < numberOfFloors; i++)
        {
            int index = i;
            Button newButton = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            newButton.onClick.AddListener(delegate { _manager.FloorTraversal.TraverseToFloor(index); }) ;
            _buttons.Add(newButton);
        }
        UpdateButtons();

    }

    void UpdateButtons(int floor = -1)
    {
        if (floor == -1)
        {
            floor = _manager.FloorTraversal.currentFloor;
        }

        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i == floor)
            {
                // get bigger
                _buttons[i].transform.localScale = Vector3.one*activeScale;
            }

            else
            {
                // get smaller
                _buttons[i].transform.localScale = (Vector3.one);

            }
        }
    }
}
