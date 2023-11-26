using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public void LoadLevel(string levelNumber)
    {
        SceneManager.LoadScene("Tutorial Level " + levelNumber);
    }

    public void LoadNewScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
