using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SplashTextInstance : MonoBehaviour
{
    private bool active;
    [Header("Status: (Read only)")]
    public float l; // lifetime

    private Vector2 startPosition;
    private RectTransform rct;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Config:")]
    public float lifeTime = 1;
    public Vector2 startPositionOffset;
    public Vector2 endPosition;
    public Vector2 endPositionRandomisation;


    // Update is called once per frame
    void Update()
    {
        // Update only runs when this is considered active
        if (!active) { return; }

        l += Time.deltaTime;

        // t for use in lerp
        float t = l / lifeTime;

        rct.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);


        if (l > lifeTime)
        {
            FinishedDisplaying();
        }
    }

    public void Init(string content, Vector3 spawnPosition)
    {
        active = true;
        transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
    
        rct = GetComponent<RectTransform>();

        startPosition = rct.anchoredPosition + startPositionOffset;

        endPosition = endPosition + startPosition;
        endPosition = new Vector2(
            endPosition.x + Random.Range(-endPositionRandomisation.x, endPositionRandomisation.x),
            endPosition.y + Random.Range(-endPositionRandomisation.y, endPositionRandomisation.y)
        );

        text.text = content;
    }

    public void FinishedDisplaying()
    {
        // Let manager know I am finished
        SplashTextManager.SplashTextFinished(this);
    
        // Should probably be object pooled
        Destroy(gameObject);
    }
}
