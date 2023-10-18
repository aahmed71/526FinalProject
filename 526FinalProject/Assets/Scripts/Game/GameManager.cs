using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private bool isPaused = false;

    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject pausePanel;

    
    [NonSerialized] public UnityEvent gameWinEvent;


    [SerializeField] private GameObject uiElement1;
    [SerializeField] private GameObject uiElement2;
    [SerializeField] private GameObject uiElement3;

  

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        gameWinEvent = new UnityEvent();
        //analytics
        // startTime = Time.time;
    }

    public void TogglePauseGame()
    {
        Debug.Log("TogglePauseGame called. Current isPaused value: " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 1;
            Debug.Log("Resuming game...");
            isPaused = false;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false); // Hide the pause panel
            }
            // Show the needed UI elements
            if (uiElement1 != null) uiElement1.SetActive(true);
            if (uiElement2 != null) uiElement2.SetActive(true);
            if (uiElement3 != null) uiElement3.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Pausing game...");
            isPaused = true;
            if (pausePanel != null)
            {
                pausePanel.SetActive(true); // Show the pause panel
            }
            // Hide the UI elements
            if (uiElement1 != null) uiElement1.SetActive(false);
            if (uiElement2 != null) uiElement2.SetActive(false);
            if (uiElement3 != null) uiElement3.SetActive(false);
        }
    }

   public void ResetGame()
    {
        Time.timeScale = 1; // Ensure game is resumed before resetting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

  public void RestartGame()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GameWin()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
        }
        gameWinEvent.Invoke();



        Invoke("RestartGame", 2f);
    }
}
