using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScreenDrawEffect : MonoBehaviour
{
    public bool active;
    public Sprite pureWhiteImage;
    public Image[] screenLines;


    private float totalLength;
    private float internalTimer;

    [Header("Config:")]
    public int scanLinesCount;
    public Color baseColor;
    public float drawTime = 2;
    public bool startWithScanLinesDrawn = false;

    void Start()
    {
        int height = (int) (Screen.height / scanLinesCount);
        
        // Monkey solution to ensure that the scan lines aren't going to be shorter than the screen:
        while (height * scanLinesCount < Screen.height)
        {
            height += 1;
        }

        screenLines = new Image[scanLinesCount];

        for (int i = 0; i < screenLines.Length; i++)
        {
            GameObject newObj = new GameObject("Scanline " + i);

            newObj.transform.SetParent(transform);
            newObj.AddComponent<Image>();

            Image newLine = newObj.GetComponent<Image>();

            // Scale and adjust pivots:
            RectTransform rct = newLine.GetComponent<RectTransform>();

            rct.localScale = Vector3.one;

            rct.pivot = new Vector2(0.5f, 1f);

            rct.anchorMin = new Vector2(0f, 1f);
            rct.anchorMax = new Vector2(1f, 1f);

            rct.anchoredPosition = new Vector2(0, height * -i);
            rct.sizeDelta = new Vector2(0, height);
            // ---
            
            
            screenLines[i] = newLine;
            newLine.sprite = pureWhiteImage;
            newLine.type = Image.Type.Filled;
            newLine.fillMethod = Image.FillMethod.Horizontal;

            newLine.color = baseColor;
            newLine.fillAmount = 1f;
            newLine.raycastTarget = true;

        }

        totalLength = scanLinesCount;

        if (!startWithScanLinesDrawn)
        {
            ResetScanLines(); // Will hide the drawn scan lines.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) { return; }
        internalTimer += Time.deltaTime;

        float t = Mathf.Lerp(0, totalLength, internalTimer / drawTime);

        int currentIndex = (int) Mathf.Clamp(t, 0, scanLinesCount-1);

        float fillAmount = t - (float)currentIndex;
        screenLines[currentIndex].fillAmount = 1 - (t - (float)currentIndex); 

        // Clamp previous scan line to zero
        if (currentIndex > 1)
        {
            screenLines[currentIndex - 2].fillAmount = 0;
            screenLines[currentIndex - 2].raycastTarget = false;

            screenLines[currentIndex - 1].fillAmount = 0;
            screenLines[currentIndex - 1].raycastTarget = false;
        }

        if (currentIndex > scanLinesCount)
        {
            active = false;
        }

    }

    public void ResetScanLines()
    {
        foreach (Image image in screenLines)
        {
            image.fillAmount = 0;
            image.raycastTarget = false;
        }

        active = false;
        internalTimer = 0;
    }

    public void StartScanDraw()
    {
        foreach (Image image in screenLines)
        {
            image.fillAmount = 1;
            image.raycastTarget = true;
        }

        active = true;
        internalTimer = 0f;
    }
}
