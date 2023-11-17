using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    int counter;
    public static int KillCount = 0;
    public TextMeshProUGUI text;
    void OnEnable()
    {
        Enemy.OnEnemyDefeated += EnemyDefeated;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDefeated -= EnemyDefeated;
    }


    void EnemyDefeated(Enemy enemy, bool escaped)
    {
        if (escaped) return;
        
        counter++;
        KillCount = counter;
        text.text = $"{counter}";
    }
}
