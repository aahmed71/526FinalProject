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
    private float sizeReductionTimer = 2.0f;
    private float sizeReductionAmount = 1.0f; // Adjust the value as needed
    private float nextSizeReductionTime;
    private bool isShrinking = false;
    private Vector3 originalScale;
    private float timeLeftOnShrinking = 0f;
    private int abilityFunctionCalls = 0; 
    public int totalAbilityFunctionCalls = 0;
    

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
        if (isShrinking)
        {
            if (Time.time >= nextSizeReductionTime)
            {
                ReducePlatformSize(platform1);
                ReducePlatformSize(platform2);
                nextSizeReductionTime = Time.time + sizeReductionTimer;
            }
        }
    }

     private void ReducePlatformSize(GameObject platform)
    {
        Vector3 currentScale = platform.transform.localScale;
        if(currentScale.x > 8){
            currentScale.x -= sizeReductionAmount;
            platform.transform.localScale = currentScale;
        }
       
    }


    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Dice");
        if (isShrinking)
        {
            // If shrinking was in progress, resume from where it left off.
            nextSizeReductionTime = Time.time + timeLeftOnShrinking;
        }
        else
        {
            // If not shrinking, start a new shrinking process.
            nextSizeReductionTime = Time.time + sizeReductionTimer;
            originalScale = platform1.transform.localScale; // Store the original scale.
            isShrinking = true;
        }
        GameManager.Instance.controlDisplay.SetText(ControlDisplay.ControlType.Ability, "Move Platforms");
        GameManager.Instance.controlDisplay.SetVisibility(ControlDisplay.ControlType.Ability, true);
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        timeLeftOnShrinking = nextSizeReductionTime - Time.time;
        isShrinking = false;
        totalAbilityFunctionCalls += abilityFunctionCalls;
       
        abilityFunctionCalls = 0;
        GameManager.Instance.controlDisplay.SetVisibility(ControlDisplay.ControlType.Ability, false);
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

            abilityFunctionCalls++;
    }
}