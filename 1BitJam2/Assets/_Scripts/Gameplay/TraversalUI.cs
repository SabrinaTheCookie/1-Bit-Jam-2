using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraversalUI : MonoBehaviour
{
    
    private FloorManager _manager;

    public GameObject buttonPrefab;

    private List<TraversalButton> _buttons = new List<TraversalButton>();

    private float activeScale = 1.5f;

    private void OnEnable()
    {
        FloorTraversal.OnTraversal += UpdateButtons;
        FloorManager.OnFloorsSetup += GenerateButtons;
        Floor.FloorNowHasEnemies += UpdateButtons;
        Floor.FloorIsNowEmpty += UpdateButtons;
    }

    private void OnDisable()
    {
        FloorTraversal.OnTraversal -= UpdateButtons;
        FloorManager.OnFloorsSetup -= GenerateButtons;
        Floor.FloorNowHasEnemies -= UpdateButtons;
        Floor.FloorIsNowEmpty -= UpdateButtons;

    }

    void GenerateButtons()
    {
        if(!_manager) _manager = FindObjectOfType<FloorManager>();

        int numberOfFloors = _manager.Floors.Count;
        for(int i = 0; i < numberOfFloors; i++)
        {
            int index = i;
            Button newButton = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            newButton.onClick.AddListener(delegate { _manager.FloorTraversal.TraverseToFloor(index); }) ;
            _buttons.Add(newButton.GetComponent<TraversalButton>());
        }
        UpdateButtons();

    }

    void UpdateButtons()
    {
        UpdateButtons(-1);
    }
    void UpdateButtons(int floor)
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
            
            _buttons[i].ToggleEnemyIcon(_manager.Floors[i].enemiesOnFloor.Count > 0);
            
        }
    }
}
