using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildTimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private BuildPhase buildPhase;
    
    // Start is called before the first frame update
    void Awake()
    {
        buildPhase = FindObjectOfType<BuildPhase>();
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int remaining = Mathf.CeilToInt(buildPhase.buildTimeRemaining);
        if (remaining >= 0)
        {
            timerText.text = remaining.ToString("00");
        }
        else if (timerText.text.Length > 0) timerText.text = "";
        

    }
}
