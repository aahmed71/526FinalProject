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
    public bool isPaused = false;

    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string nextLevelName = null;

    
    [NonSerialized] public UnityEvent gameWinEvent;


    [SerializeField] public GameObject Win;
    [SerializeField] public GameObject Lose;
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject PauseButton;
    [SerializeField] public GameObject RestartButton;

  

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
            if (Player != null) Player.SetActive(true);
            if (RestartButton != null) RestartButton.SetActive(false);
            if (PauseButton != null) PauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
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
            if (Player != null) Player.SetActive(false);
            if (RestartButton != null) RestartButton.SetActive(true);
            if (PauseButton != null) PauseButton.GetComponentInChildren<TMP_Text>().text = "Resume";
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

  public void LoadNextLevel()
  {
      if (winText != null)
      {
          winText.gameObject.SetActive(false);
      }
      // Load the Next Scene
      if (nextLevelName != null)
      {
          SceneManager.LoadScene(nextLevelName);
      }
  }

    public void GameWin()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
        }
        gameWinEvent.Invoke();



        Invoke("LoadNextLevel", 2f);
    }
}
