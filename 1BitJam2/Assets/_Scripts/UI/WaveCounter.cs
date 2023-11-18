using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    public EnemyWaveManager enemyWaveManager;
    public TextMeshProUGUI text;

    void OnEnable()
    {
        EnemyWaveManager.OnWaveComplete += WaveCompleted;
    }

    void OnDisable()
    {
        EnemyWaveManager.OnWaveComplete -= WaveCompleted;
    }


    void WaveCompleted()
    {
        text.text = $"{enemyWaveManager.waveNumber}";
    }
}
