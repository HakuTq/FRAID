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
    [SerializeField] PlayerMainScript playerMainScript;
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
        Debug.Log(readyToAttack);
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
        distanceBetweenPlayerAndBoss = Vector2.Distance(transform.position, playerMainScript.transform.position);
        if (readyToAttack && distanceBetweenPlayerAndBoss < 2)
        {
            readyToAttack = false;
            //rb.velocity = Vector2.zero;
            animator.SetTrigger("attack");
        }
        else if (transform.position.x > playerMainScript.transform.position.x)
        {
            WalkingLeft();
        }
        else WalkingRight();
    }

    void BossDeath()
    {
        Destroy(rb);
    }

    public void WalkingLeft()
    {
        rb.velocity = Vector2.left * walkingSpeed;
        animator.SetBool("isWalking", true);
        sprite.flipX = false;
    }

    public void WalkingRight()
    {
        rb.velocity = Vector2.right * walkingSpeed;
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
