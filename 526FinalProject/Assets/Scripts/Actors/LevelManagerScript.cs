using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public void LoadLevel(string levelNumber)
    {
        Time.timeScale=1;
        SceneManager.LoadScene("Tutorial Level " + levelNumber);
    }

    public void LoadNewScene(string sceneName){
        Time.timeScale=1;
        SceneManager.LoadScene(sceneName);
    }
}
