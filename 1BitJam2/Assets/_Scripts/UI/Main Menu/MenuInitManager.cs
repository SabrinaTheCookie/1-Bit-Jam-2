using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInitManager : MonoBehaviour
{
    // Provides stage diretion for the main menu:
    public float initialWaitTime = 1;
    public ScreenDrawEffect screenDraw;

    public CanvasGroup openingCanvas;
    public CanvasGroup mainCanvas;

    private bool finishedRenderingIntroduction = false;
    bool mainMenuShown;

    public void Start()
    {
        StartCoroutine(GameOpening());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (finishedRenderingIntroduction &! mainMenuShown)
            {
                AudioManager.Instance.PlaySound("UiBlip");
                StartCoroutine(ShowMainMenu());
            }
        }
    }

    public IEnumerator GameOpening()
    {
        openingCanvas.alpha = 1f;
        openingCanvas.blocksRaycasts = true;

        mainCanvas.alpha = 0f;
        mainCanvas.blocksRaycasts = false;

        yield return new WaitForSeconds(initialWaitTime);

        screenDraw.StartScanDraw();

        yield return new WaitForSeconds(2f); // Wait for the screen to render
        finishedRenderingIntroduction = true;

        // Wait for space input (bad bad bad code)
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return new WaitForEndOfFrame();
            // :3
        }
        

    }

    public IEnumerator ShowMainMenu()
    {
        mainMenuShown = true;
        screenDraw.ResetScanLines();
        screenDraw.StartScanDraw();

        openingCanvas.alpha = 0f;
        openingCanvas.blocksRaycasts = false;

        mainCanvas.alpha = 1f;
        mainCanvas.blocksRaycasts = true;

        yield return new WaitForSeconds(initialWaitTime);
    }
}
