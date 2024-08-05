using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityMeterScript : MonoBehaviour
{
    [SerializeField] private float maxAbilityMeter;
    [SerializeField] private float startingAbilityMeter;
    [SerializeField] private bool abilityReady;
    UI_PlayerHealth uiPlayerHealth;
    private float abilityMeter = 0;

    public float MaxAbilityMeter
    {
        get { return maxAbilityMeter; }
        set 
        { 
            maxAbilityMeter = value;
            uiPlayerHealth.UpdateAbilityBarMax(value);
        }
    }

    public float AbilityMeter
    {
        get { return abilityMeter; }
        set
        {
            if (value > maxAbilityMeter)
            {
                abilityMeter = MaxAbilityMeter; //nemùže pøekroèit maximum
                uiPlayerHealth.UpdateAbilityBar(MaxAbilityMeter);
            }
            else
            {
                abilityMeter = value;
                uiPlayerHealth.UpdateAbilityBar(value);
            }
        }
    }

    public bool AbilityReady
    {
        get { return abilityReady; }
    }

    private void Start()
    {
        uiPlayerHealth = FindAnyObjectByType<UI_PlayerHealth>();
        if (uiPlayerHealth == null) Debug.Log("!WARNING! Script AbilityMeterScript couldnt find the UI_PlayerHealth script");
        AbilityMeter += startingAbilityMeter;
    }

    private void Update() //Øeší jestli je ability Pøipravena
    {
        if (AbilityMeter == MaxAbilityMeter) abilityReady = true;
        else abilityReady = false;
    }

    public void SetAbilityReady() //Nastaví tak aby byla abilitka ready
    {
        AbilityMeter = MaxAbilityMeter;
    }
}
