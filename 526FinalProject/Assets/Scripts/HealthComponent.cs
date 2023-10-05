using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    // health
    [SerializeField] private float maxHealth;
    private float currentHealth;
    
    // Optional UI
    [SerializeField] private TextMeshProUGUI healthCounter;
    
    //variables to tweak for death behavior
    [SerializeField] private float deathDelay;

    //event in case we want anything specific to happen on death
    [NonSerialized] public UnityEvent deathEvent;
    
    void Start()
    {
        //creates new even if there isn't one
        if (deathEvent == null)
        {
            deathEvent = new UnityEvent();
        }
        
        //sets current health
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        //if there's health ui, update it
        if (healthCounter)
        {
            healthCounter.text = "Health: " + currentHealth.ToString("0.00");
        }
    }
    
    public void TakeDamage(float damage)
    {
        UpdateHealthText();
        
        //take damage
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());
        }
    }
    
    IEnumerator Die()
    {
        //calls event in case there's anything specific we want called
        deathEvent.Invoke();

        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
