using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealth : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] AbilityMeterScript abilityMeterScript;
    [SerializeField] GameObject[] healthImage;
    [SerializeField] GameObject[] emptyHealthImage;
    [SerializeField] Slider abilitySlider;


    private void Start()
    {
        ResetHealthUI();
        SetHealthUI();
    }
    public void SetHealthUI()
    {
        //hp1
        if (health.Health > 0)
        {
            healthImage[0].SetActive(true);
            emptyHealthImage[0].SetActive(false);
        }
        else
        {
            healthImage[0].SetActive(false);
            emptyHealthImage[0].SetActive(true);
        }
        //hp2
        if (health.Health > 1)
        {
            healthImage[1].SetActive(true);
            emptyHealthImage[1].SetActive(false);
        }
        else
        {
            healthImage[1].SetActive(false);
            emptyHealthImage[1].SetActive(true);
        }
        //hp3
        if (health.Health > 2)
        {
            healthImage[2].SetActive(true);
            emptyHealthImage[2].SetActive(false);
        }
        else
        {
            healthImage[2].SetActive(false);
            emptyHealthImage[2].SetActive(true);
        }
        //hp4
        if (health.MaxHealth > 3) //4th Health Is Invisible
        {
            if (health.Health > 3) healthImage[3].SetActive(true);
            else emptyHealthImage[3].SetActive(true);
        }
    }

    public void ResetHealthUI()
    {
        foreach (GameObject image in healthImage) image.SetActive(false);
        foreach (GameObject emptyImage in emptyHealthImage) emptyImage.SetActive(false);
    }

    public void UpdateAbilityBar(float value)
    {
        abilitySlider.value = value;
    }

    public void UpdateAbilityBar(float value, float max)
    {
        abilitySlider.value = value;
        abilitySlider.maxValue = max;
    }

    public void UpdateAbilityBarMax(float max)
    {
        abilitySlider.maxValue = max;
    }
}
