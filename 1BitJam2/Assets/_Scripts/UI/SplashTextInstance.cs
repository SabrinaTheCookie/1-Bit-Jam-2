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
    [Header("Position over time:")]
    public Vector2 startPositionOffset;
    public Vector2 endPosition;
    public Vector2 endPositionRandomisation;
    [Header("Scaling over time:")]
    public float scalingStartTime = 0.5f;
    public Vector2 startScaleFactor;
    private Vector2 startScale;
    public Vector2 endScaleFactor;
    private Vector2 endScale;


    // Update is called once per frame
    void Update()
    {
        // Update only runs when this is considered active
        if (!active) { return; }

        l += Time.deltaTime;

        // t for use in lerp
        float t = l / lifeTime;

        rct.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

        // For use in scaling
        float scalingT = ((l- scalingStartTime) / lifeTime) * (1/scalingStartTime);
        rct.sizeDelta = Vector2.Lerp(startScale, endScale, scalingT);

        if (l > lifeTime)
        {
            FinishedDisplaying();
        }
    }

    public void Init(string content, Vector3 spawnPosition)
    {
        active = true;
        rct = GetComponent<RectTransform>();
        Vector3 viewport = Camera.main.WorldToViewportPoint(spawnPosition);
        viewport.x *= Screen.width;
        viewport.y *= Screen.height;

        transform.position = viewport;
    

        startPosition = rct.anchoredPosition + startPositionOffset;

        endPosition = endPosition + startPosition;
        endPosition = new Vector2(
            endPosition.x + Random.Range(-endPositionRandomisation.x, endPositionRandomisation.x),
            endPosition.y + Random.Range(-endPositionRandomisation.y, endPositionRandomisation.y)
        );

        text.text = content;

        startScale = rct.sizeDelta * startScaleFactor;
        endScale = rct.sizeDelta * endScaleFactor;
    }

    public void FinishedDisplaying()
    {
        // Let manager know I am finished
        SplashTextManager.SplashTextFinished(this);
    
        // Should probably be object pooled
        Destroy(gameObject);
    }
}
