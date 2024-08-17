using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] UI_PlayerHealth uiPlayerHealth;
    [SerializeField] UI_PlayerDeath uiPlayerDeath;
    Animator animator;
    [Header("Hodnoty")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float timerInvincibility;
    [SerializeField] private bool invincible; 
    [SerializeField] private bool godMode; //Is not affected by timer (Like invincible)
    public float Health
    {
        get { return health; }
        set 
        {
            if (value == 0) //Player Death
            {
                StartCoroutine(StopPlayingAnimator()); //Animator Plays Death Animation
                uiPlayerDeath.PlayerDeathUI();                
            }                      
            if (value > maxHealth) health = maxHealth; //Current Health cannot be bigger than MaxHealth
            else health = value;
            uiPlayerHealth.SetHealthUI(); //Set UI
        }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float InvincibilityTime //Pochybuju že se s tím bude manipulovat ale tak pro jistotku
    {
        get { return invincibilityTime; }
        set { invincibilityTime = value; } 
    }

    public bool GodMode
    {
        get { return godMode; }
        set { godMode = value; }
    }

    public void PlayerSetInvincible() //Zapne odpoèet pro invincibility frames
    {
        invincible = true;
        timerInvincibility = invincibilityTime;
    }
    /// <summary>
    /// Can turn off invincibility and the number of the damage
    /// </summary>
    /// <param name="setInvincibility"></param>
    /// <param name="damage"></param>
    public void PlayerDamage(bool setInvincibility, float damage)
    {
        if (!invincible && !godMode) Health -= damage;
        if (setInvincibility) PlayerSetInvincible();
    }
    /// <summary>
    /// Damage is 1, can turn off invincibility
    /// </summary>
    /// <param name="setInvincibility"></param>
    public void PlayerDamage(bool setInvincibility)
    {
        if (!invincible && !godMode) Health -= 1;
        if (setInvincibility) PlayerSetInvincible();
    }
    /// <summary>
    /// Damage is 1, invincibility is turned on
    /// </summary>
    public void PlayerDamage()
    {
        if (!invincible && !godMode)
        {
            Health -= 1;
            PlayerSetInvincible();
            Debug.Log("Player Has Taken Damage");
        }        
    }

    public void PlayerMaxHealth()
    {
        Health = MaxHealth;
    }

    public void PlayerHeal()
    {
        Health += 1;
    }

    public void PlayerDeath()
    {
        Health = 0;
    }
    private void Start()
    {
        if (uiPlayerHealth == null) Debug.Log("!ERROR! Script PlayerHealth couldnt get the UI_PlayerHealth script");
        if (uiPlayerDeath == null) Debug.Log("!ERROR! Script PlayerHealth couldnt get the UI_PlayerDeath script");
        animator = GetComponentInParent<Animator>();
        if (animator == null) Debug.Log("!ERROR! Animator in script PlayerHealth couldnt find the Animator");
        Health = MaxHealth;
    }
    private void Update()
    {
        if (invincible) //Invincibility frames timer
        {
            timerInvincibility -= Time.deltaTime;
            if (timerInvincibility < 0)
            {
                invincible = false;
                timerInvincibility = invincibilityTime;
            }
        }
    }

    IEnumerator StopPlayingAnimator() //Because what the fuck is a Stop Playing Function
    {
        animator.SetTrigger("PlayerDeath");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.speed = 0;
    }


}
