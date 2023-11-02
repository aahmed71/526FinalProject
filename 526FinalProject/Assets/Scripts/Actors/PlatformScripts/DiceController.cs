using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceController : EntityController
{
  
    public TextMeshProUGUI textDisplay;
    public string[] textNumbers; 
    public GameObject platform1; 
    public GameObject platform2; 
    public Vector3[] platform1Positions; 
    public Vector3[] platform2Positions; 

    private int currentState = 0; 

    protected override void Start()
    {
        base.Start(); 
        textDisplay.text = textNumbers[0]; 

        if (platform1 != null)
        {
            platform1.transform.position = platform1Positions[0];
        }

        if (platform2 != null)
        {
            platform2.transform.position = platform2Positions[0];
        }
    }
    
    public override void Update()
    {
        base.Update();
    }


    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        // GameManager.Instance.CalculatePosessionCount("Lighter");
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        // GameManager.Instance.CalculateUnPosessionCount("Lighter");
    }

    protected override void Ability()
    {
        currentState = (currentState + 1) % textNumbers.Length;

            
            textDisplay.text = textNumbers[currentState];


            if (platform1 != null && currentState < platform1Positions.Length)
            {
                platform1.transform.position = platform1Positions[currentState];
            }

            if (platform2 != null && currentState < platform2Positions.Length)
            {
                platform2.transform.position = platform2Positions[currentState];
            }
    }
}
