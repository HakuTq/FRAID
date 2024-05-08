using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float invincibilityTime;
    private float timerInvincibility;
    [SerializeField] private bool invincible; //nastavuje se pøes metodu
    [SerializeField] private bool godMode; //nevztahuje se na nìj èasovaè
    private bool isDead = false;

    public float Health
    {
        get { return health; }
        set 
        {            
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

    public bool IsDead
    {
        get { return isDead; }
    }

    public void PlayerSetInvincible() //Zapne odpoèet pro invincibility frames
    {
        invincible = true;
        timerInvincibility = invincibilityTime;
    }

    public void PlayerDamage(bool setInvincibility, float damage)
    {
        if (!invincible && !godMode) health -= damage;
        if (setInvincibility) PlayerSetInvincible();
    }

    public void PlayerMaxHealth()
    {
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

        if (health <= 0) isDead = true; //Kontrola smrti
    }


}
