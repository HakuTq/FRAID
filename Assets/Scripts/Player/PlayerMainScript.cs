using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    PlayerHealth playerHealth;

    public bool PlayerTriggerDamage //I mean if it works it works
    {
        set 
        { 
            if (value == true)
            {
                if (playerHealth != null) playerHealth.PlayerDamage();
                else Debug.Log("!ERROR! Could not load PlayerHealth");
            }            
        }
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    
}
