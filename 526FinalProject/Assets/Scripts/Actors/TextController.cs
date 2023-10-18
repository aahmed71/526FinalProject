using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class TextController : MonoBehaviour

{
   
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    void Start()
    {
        text1.enabled = false;
        text2.enabled = false;

        StartCoroutine(DisplayText());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

     IEnumerator DisplayText()
    {

        yield return new WaitForSeconds(2f); // Wait for 1 second

        // Display the first text
        text1.enabled = true;

        yield return new WaitForSeconds(2f); // Wait for 1 second

        // Display the second text
        text2.enabled = true;

        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        // Hide both texts
        text1.enabled = false;
        text2.enabled = false;
    }
}
