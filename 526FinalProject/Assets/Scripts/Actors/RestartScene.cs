using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public string scene = "RiddhiTest";
    public void RestartSceneNext()
    {
        // Reloads the current scene
        SceneManager.LoadScene(scene);
    }
}
