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
    
    //Debug Commands
    private string level1 = "Tutorial Level 1";
    private string level2 = "Tutorial Level 2";
    private string level3 = "Tutorial Level 3";

    //analytics
    private bool playerLose = false;
    private float pTime;
    private float upTime;
    private int CheckPoint;
    private float checkpointTime;
    private int spawnCount;
    Dictionary<string, int> possessionCount = new Dictionary<string, int>();
    Dictionary<string, int> deathDict = new Dictionary<string, int>();
    public DiceController diceController; 

    private string platform;
    private int platformChangeCount;


    //end of analytics
    
    [SerializeField] private string nextLevelName = null;

    
    [NonSerialized] public UnityEvent gameWinEvent;

    [SerializeField] public GameObject Pause;
    [SerializeField] public GameObject Win;
    [SerializeField] public GameObject Lose;
    [SerializeField] public GameObject PauseButton;
    


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
        string currentLevelName = SceneManager.GetActiveScene().name;
        FindObjectOfType<GoogleAnalytics>().CreateSession();
        if (currentLevelName == "Tutorial Level 1")
        {
            // Call a function in the analytics script
            FindObjectOfType<GoogleAnalytics>().LevelNumber(1);
        }
        else if(currentLevelName == "Tutorial Level 2")
        {
            FindObjectOfType<GoogleAnalytics>().LevelNumber(2);
        }
        else if(currentLevelName == "Tutorial Level 3")
        {
            FindObjectOfType<GoogleAnalytics>().LevelNumber(3);
        }
        else if(currentLevelName == "Tutorial Level 4")
        {
            FindObjectOfType<GoogleAnalytics>().LevelNumber(4);
        }
        else{
            FindObjectOfType<GoogleAnalytics>().LevelNumber(5);
        }

        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("Running on webgl");
            platform = "WebGL";
        }
        else
        {
           
            Debug.Log("Running on a different platform.");
            Debug.Log(Application.platform.ToString());
            platform = Application.platform.ToString();
        }

        if (GameObject.Find("Dice"))
        {
            diceController = GameObject.Find("Dice").GetComponent<DiceController>();
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
            // Show the needed UI elements
            if (Pause!=null) Pause.SetActive(false);
            // if (PauseButton != null) PauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Pausing game...");
            isPaused = true;
            // Hide the UI elements
            if (Pause!=null) Pause.SetActive(true);
            // if (PauseButton != null) PauseButton.GetComponentInChildren<TMP_Text>().text = "Resume";
        }
    }

    public void CalculatePosessionCount(string objname){
        Debug.Log("In Possession Count");
        if(possessionCount.ContainsKey(objname)){
            possessionCount[objname]++;
        }else{
             possessionCount[objname] = 1;
        }
        
    }

    public void CountBombSpawns(){
        spawnCount++;
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

    


  public void LoadNextLevel()
  {
      Win.SetActive(false);
      Time.timeScale=1;
      // Load the Next Scene
      if (nextLevelName != null)
      {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        checkpointTime = playerController.timeToReachCheckpoint;
        deathDict = playerController.deathCountDict;
        if(diceController!=null){
            platformChangeCount = diceController.totalAbilityFunctionCalls;
            Debug.Log("platform count" + platformChangeCount);
        }else{
            platformChangeCount = 0;
        }
        
        FindObjectOfType<GoogleAnalytics>().Send(deathDict,platform,possessionCount,1,checkpointTime,spawnCount,platformChangeCount);
        SceneManager.LoadScene(nextLevelName);
      }
  }

    public void GameWin()
    {
        Win.SetActive(true);
        Time.timeScale=0;
        gameWinEvent.Invoke();
       
    }


    public void GameLose()
    {

        if(!playerLose){
            
            playerLose = true;
            PauseButton.SetActive(false);
            Lose.SetActive(true);

            PlayerController playerController = FindObjectOfType<PlayerController>();
            deathDict = playerController.deathCountDict;
            if(diceController!=null){
            platformChangeCount = diceController.totalAbilityFunctionCalls;
            Debug.Log("platform count" + platformChangeCount);
            }else{
                platformChangeCount = 0;
            }
            Debug.Log("platform count" + platformChangeCount);

            if (playerController != null)
            {
                if(playerController.hasReachedCheckpoint){
                    CheckPoint = 1;
                    checkpointTime = playerController.timeToReachCheckpoint;
                    Debug.Log("Checkpoint time in game manager" + checkpointTime);
                }else{
                    CheckPoint = 0;
                    checkpointTime = 0.0f;
                }
            }
            
            
            FindObjectOfType<GoogleAnalytics>().Send(deathDict,platform,possessionCount,CheckPoint,checkpointTime,spawnCount,platformChangeCount);
          
            
        }
    }

    public void BackToLevelSelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelsSwitcher");
    }
}
