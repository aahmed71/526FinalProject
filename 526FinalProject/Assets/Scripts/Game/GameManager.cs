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

    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private TextMeshProUGUI winText;
    
    //event in case we want anything specific to happen on death
    [NonSerialized] public UnityEvent gameWinEvent;
    
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
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
    }
    
    void RestartGame()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        // Reload the current scene (you can specify the scene name or index)
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
