using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    public TextMeshProUGUI towerInfoText;
    public float messageDuration;

    private void OnEnable()
    {
        TowerSpawner.OnTowerSelected += UpdateTowerInfo;
        TowerSpawner.OnTowerPlacementMessage += TowerPlacementMessage;
    }
    
    private void OnDisable()
    {
        TowerSpawner.OnTowerSelected -= UpdateTowerInfo;
        TowerSpawner.OnTowerPlacementMessage -= TowerPlacementMessage;
    }

    void UpdateTowerInfo(string info)
    {
        StopCoroutine(ClearMessage());
        towerInfoText.text = info;
    }

    void TowerPlacementMessage(string message)
    {
        towerInfoText.text = message;
        StartCoroutine(ClearMessage());
    }

    IEnumerator ClearMessage()
    {
        yield return new WaitForSeconds(messageDuration);
        towerInfoText.text = "";
        yield return null;
    }
}
