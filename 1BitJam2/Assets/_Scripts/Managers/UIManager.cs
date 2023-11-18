using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverWinLossText;
    public TextMeshProUGUI gameOverScore;
    
    private void OnEnable()
    {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        if(GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameOver -= OnGameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameOver += OnGameOver;
    }
    
    void OnGameOver()
    {
        // Bad code I know, but time saving with only hours left :P
        transform.GetChild(1).gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != 2) { SceneManager.LoadScene(2); }

        StartCoroutine(GameOverScreen());
        /*menu.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverWinLossText.text = GameStateManager.Instance.GetGameState().isVictorious ? "Victory" : "Defeat";
        gameOverScore.text = "Waves Survived: " + GameStateManager.Instance.GetGameState().score.ToString("0");*/
    }


    IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(28f);

        menu.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverWinLossText.text = GameStateManager.Instance.GetGameState().isVictorious ? "Victory" : "Defeat";
        gameOverScore.text = "Waves Survived: " + GameStateManager.Instance.GetGameState().score.ToString("0");
    }


    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
