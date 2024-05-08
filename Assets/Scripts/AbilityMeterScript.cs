using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityMeterScript : MonoBehaviour
{
    [SerializeField] private float maxAbilityMeter;
    [SerializeField] private float startingAbilityMeter;
    [SerializeField] private bool abilityReady;
    private float abilityMeter = 0;

    public float MaxAbilityMeter
    {
        get { return maxAbilityMeter; }
        set { maxAbilityMeter = value; }
    }

    public float AbilityMeter
    {
        get { return abilityMeter; }
        set
        {
            if (value > maxAbilityMeter) abilityMeter = MaxAbilityMeter; //nem��e p�ekro�it maximum
            else abilityMeter = value;
        }
    }

    public bool AbilityReady
    {
        get { return abilityReady; }
    }

    private void Start()
    {
        AbilityMeter += startingAbilityMeter;
    }

    private void Update() //�e�� jestli je ability P�ipravena
    {
        if (AbilityMeter == MaxAbilityMeter) abilityReady = true;
        else abilityReady = false;
    }

    public void SetAbilityReady() //Nastav� tak aby byla abilitka ready
    {
        AbilityMeter = MaxAbilityMeter;
    }
}
