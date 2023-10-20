using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpInstructionController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textMeshPro;
    void Start()
    {
        
        StartCoroutine(ShowAndHideText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShowAndHideText()

    {
        textMeshPro.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        // Show the TextMeshPro text
        textMeshPro.gameObject.SetActive(true);

        // Wait for 7 seconds (total of 10 seconds)
        yield return new WaitForSeconds(7f);

        // Hide the TextMeshPro text
        textMeshPro.gameObject.SetActive(false);
    }
}
