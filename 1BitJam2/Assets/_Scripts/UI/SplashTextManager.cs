#define DEBUG_INPUT
// Comment out the above line to disable debug input

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SplashTextManager : MonoBehaviour
{
    private static SplashTextManager instance;
    


    public enum SplashTextStyle
    {
        Damage,
        Currency
    }
    [SerializeField] private static List<SplashTextInstance> splashTexts;

    
    public static GameObject damageSplash;
    public static GameObject currencySplash;



    void Awake()
    {
        // Init
        instance = this;
        splashTexts = new List<SplashTextInstance>();

        // Load splashtexts:
        damageSplash = Resources.Load<GameObject>("SplashText/SplText_Damage");
        currencySplash = Resources.Load<GameObject>("SplashText/SplText_Currency");
    }

    #if DEBUG_INPUT
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnSplashText(SplashTextStyle.Currency, "+5", Vector3.one);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnSplashText(SplashTextStyle.Damage, "-15", Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ClearAllSplashTexts();
        }
    }
    #endif

    private static void CheckInstance()
    {
        if (instance == null)
        {
            Debug.LogError("No instance of SplashTextManager found, Please ensure that an instance is in the scene");
            return;
        }
    }

    /// <summary>
    /// Spawns in a floating damage text at a certain point on the screen.
    /// </summary>
    /// <param name="damage">The damage taken</param>
    /// <param name="position">The position that the damage numbers should spawn</param>
    public static void SpawnSplashText(SplashTextStyle style, string content, Vector3 position)
    {
        // Ensure an instance exists
        CheckInstance();
    
        GameObject newObject = Instantiate(GetObjectFromStyleEnum(style), instance.transform);
        SplashTextInstance newSplashText = newObject.GetComponent<SplashTextInstance>();

        splashTexts.Add(newSplashText);

        newSplashText.Init(content, position);
    }

    /// <summary>
    /// Will manually force all splash text instances to stop what they are doing and dissapear
    /// </summary>
    public static void ClearAllSplashTexts()
    {
        // Ensure an instance exists
        CheckInstance();

        // Copy list to prevent error from modification of foreach loop as the texts remove themselves
        SplashTextInstance[] splashTextsToNotify = new SplashTextInstance[splashTexts.Count];
        splashTexts.CopyTo(splashTextsToNotify);

        foreach(SplashTextInstance ST in splashTextsToNotify)
        {
            ST.FinishedDisplaying();
        }
    }

    // Called when a splash text has finished it's lifetime
    public static void SplashTextFinished(SplashTextInstance finishedText)
    {
        // Ensure an instance exists
        CheckInstance();

        splashTexts.Remove(finishedText);
    }

    private static GameObject GetObjectFromStyleEnum(SplashTextStyle style)
    {
        switch (style)
        {
            case SplashTextStyle.Damage:
                return damageSplash;
            case SplashTextStyle.Currency:
                return currencySplash;
        }

        return null;
    }
}
