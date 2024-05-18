using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEmemy : MonoBehaviour
{
    //[SerializeField] Player Player; --PlayerScript
    [SerializeField] float maxHealth;
    [SerializeField] float nextAttackTime;
    private bool attacked;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    BoxCollider2D collider;
    float timerAttack = 0;
    bool readyToAttack = true;
    double health;

    public bool ReadyToFire
    {
        get { return readyToAttack; }
    }

    public bool Attacked
    {
        get { return attacked; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        health = maxHealth;
    }
    void Update()
    {
        if (health <= 0) EnemyDeath();
        if (!readyToAttack)
        {
            timerAttack += Time.deltaTime;
            if (timerAttack > nextAttackTime)
            {
                readyToAttack = true;
                timerAttack = 0;
            }
        }
    }

    public void Attack()
    {        
        readyToAttack = false;
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            //health -= weaponDamage;
        }
    }
}
