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
  
    Dictionary<string, int> possessionCount = new Dictionary<string, int>();
    Dictionary<string, int> unPossessionCount = new Dictionary<string, int>();

    private string platform;


    //end of analytics
    
    [SerializeField] private GameObject pausePanel;
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

    public void CalculatePosessionCount(string objname){
        Debug.Log("In Possession Count");
        if(possessionCount.ContainsKey(objname)){
            possessionCount[objname]++;
        }else{
             possessionCount[objname] = 1;
        }
        
    }

     public void CalculateUnPosessionCount(string objname){
        Debug.Log("In UnPossession Count");
        if(unPossessionCount.ContainsKey(objname)){
            unPossessionCount[objname]++;
        }else{
            unPossessionCount[objname] = 1;
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
        FindObjectOfType<GoogleAnalytics>().Send(0,0,platform,possessionCount,unPossessionCount);
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
            foreach (var kvp in possessionCount)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
            foreach (var kvp in unPossessionCount)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
            if(s=="sp"){
                FindObjectOfType<GoogleAnalytics>().Send(0,1,platform,possessionCount,unPossessionCount);
            }else{
                FindObjectOfType<GoogleAnalytics>().Send(1, 0,platform,possessionCount,unPossessionCount);
            }
            
        }
    }
}
