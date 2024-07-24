using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] UI_PlayerHealth ui_PlayerHealth;
    [SerializeField] UI_PlayerDeath ui_PlayerDeath;
    Animator animator;
    [Header("Hodnoty")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float timerInvincibility;
    [SerializeField] private bool invincible; //nastavuje se pøes metodu
    [SerializeField] private bool godMode; //nevztahuje se na nìj èasovaè
    public float Health
    {
        get { return health; }
        set 
        {
            if (value == 0)
            {
                animator.SetTrigger("death");
                ui_PlayerDeath.PlayerDeathUI();                
            }
            ui_PlayerHealth.SetHealthUI();
            if (value > maxHealth) health = maxHealth; //Životy nemùžou pøekroèit maximální životy
            else health = value;
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
        if (!invincible && !godMode) health -= damage;
        if (setInvincibility) PlayerSetInvincible();
    }
    /// <summary>
    /// Damage is 1, can turn off invincibility
    /// </summary>
    /// <param name="setInvincibility"></param>
    public void PlayerDamage(bool setInvincibility)
    {
        if (!invincible && !godMode) health -= 1;
        if (setInvincibility) PlayerSetInvincible();
    }
    /// <summary>
    /// Damage is 1, invincibility is turned on
    /// </summary>
    public void PlayerDamage()
    {
        if (!invincible && !godMode) health -= 1;
        PlayerSetInvincible();
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
        animator = GetComponent<Animator>();
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


}
