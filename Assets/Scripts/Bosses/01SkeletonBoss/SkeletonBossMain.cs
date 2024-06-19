using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBossMain : MonoBehaviour
{
    //INSPECTOR
    [SerializeField] float maxHealth;
    [SerializeField] float damageWeapon;
    [SerializeField] float walkingSpeed;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float damage;
    //COMPONENTS
    Rigidbody2D rb;
    Collider2D collider;
    Animator animator;
    SpriteRenderer sprite;
    //SCRIPTS
    //Variables
    float health;
    float timer_timeBetweenAttacks = 0;
    bool readyToAttack = false;
    float distanceBetweenPlayerAndBoss;
    bool faceLeft = true; //true == left, false == right
    enum Phase
    {
        first,
        second
    }
    Phase currentPhase;

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        currentPhase = Phase.first;
    }
    private void Update()
    {
        //timers
        if (!readyToAttack)
        {
            timer_timeBetweenAttacks += Time.deltaTime;
            if (timer_timeBetweenAttacks > timeBetweenAttacks)
            {
                readyToAttack = true;
                timer_timeBetweenAttacks = 0;
            }
        }
        //health
        if (health <= 0) BossDeath();
        //Attack
        if (readyToAttack && distanceBetweenPlayerAndBoss < 2) //random ass unit
        {
            animator.SetTrigger("attack");
        }
        //
        //if (player.transform.position.x > transorm.position.x) faceLeft = true; --kdyby se nahodou nekdy pouzivalo
        //else faceLeft = false; 
        if (faceLeft) sprite.flipX = false;
        else sprite.flipX = true;
        //distanceBetweenPlayerAndBoss = Vector2.Distance(transform.position, playerScript)
    }

    void BossDeath()
    {
        Destroy(rb);
    }

    public void WalkingLeft()
    {
        transform.position = Vector2.left * walkingSpeed * Time.deltaTime;
        animator.SetBool("isWalking", true);
        sprite.flipX = false;
    }

    public void WalkingRight()
    {
        transform.position = Vector2.right * walkingSpeed * Time.deltaTime;
        animator.SetBool("isWalking", true);
        sprite.flipX = true;  
    }

    public void Damage(float value)
    {
        health -= value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerAttack") Damage(damage);
    }
}
