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

    //analytics
    private bool playerLose = false;
    private float pTime;
    private float upTime;

    //end of analytics
    
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string nextLevelName = null;

    
    [NonSerialized] public UnityEvent gameWinEvent;

    [SerializeField] public GameObject Pause;
    [SerializeField] public GameObject Win;
    [SerializeField] public GameObject Lose;
    [SerializeField] public GameObject PauseButton;
    [SerializeField] public SaveScriptableObject SaveData;



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
        if (SaveData.SessionID == "")
        {
            Debug.Log("CREATE NEW SESSION");
            SaveData.SessionID = "newSession";
        }

        string currentLevelName = SceneManager.GetActiveScene().name;
        FindObjectOfType<GoogleAnalytics>().CreateSession();
        if (currentLevelName == "RiddhiTest")
        {
            // Call a function in the analytics script
            FindObjectOfType<GoogleAnalytics>().LevelNumber(1);
        }
        else if(currentLevelName == "Level2")
        {
            FindObjectOfType<GoogleAnalytics>().LevelNumber(2);
        }
        else{
            FindObjectOfType<GoogleAnalytics>().LevelNumber(3);
        }
     
        //end of analytics
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
            if (Pause!=null) Pause.SetActive(false);
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
            if (Pause!=null) Pause.SetActive(true);
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
        Time.timeScale = 1;
        Win.SetActive(false);
        Lose.SetActive(false);
        Pause.SetActive(false);
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void BlockPossessTime(float posessTime){
        Debug.Log("In block possess time");
        pTime = posessTime;
    }

    public void BlockUnPossessTime(float unpossessTime){
        Debug.Log("In Block unpossess time");
        upTime = unpossessTime - pTime;
        Debug.Log(upTime);
        FindObjectOfType<GoogleAnalytics>().BlockMechanics(upTime);

    }

  public void LoadNextLevel()
  {
      Win.SetActive(false);
      // Load the Next Scene
      if (nextLevelName != null)
      {
        FindObjectOfType<GoogleAnalytics>().Send(0,0);
        SceneManager.LoadScene(nextLevelName);
      }
  }

    public void GameWin()
    {
        Win.SetActive(true);
        gameWinEvent.Invoke();
       
    }


    public void GameLose(string s)
    {

        if(!playerLose){
            
            playerLose = true;
            PauseButton.SetActive(false);
            Lose.SetActive(true);
            //analytics
            if(s=="sp"){
                FindObjectOfType<GoogleAnalytics>().Send(0,1);
            }else{
                 FindObjectOfType<GoogleAnalytics>().Send(1, 0);
            }
            
        }
    }
}
