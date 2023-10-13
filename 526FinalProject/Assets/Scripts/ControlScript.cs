using UnityEngine;
using UnityEngine.UI;

public class ControlDisplay : MonoBehaviour
{
    public Text wasdText;
    public Text spaceText;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            wasdText.color = Color.green;
        }
        else
        {
            wasdText.color = Color.white;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            spaceText.color = Color.green;
        }
        else
        {
            spaceText.color = Color.white;
        }
    }
}
