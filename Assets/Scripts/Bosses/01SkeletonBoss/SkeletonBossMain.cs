using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBossMain : MonoBehaviour
{
    // INSPECTOR
    [SerializeField] float maxHealth;
    [SerializeField] float damageWeapon;
    [SerializeField] float walkingSpeed;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float damage;
    [SerializeField] float distanceToAttack;
    [SerializeField] float distanceToIdle; //has to be lower than distanceToAttack
    // COMPONENTS
    Rigidbody2D rb;
    Collider2D collider;
    Animator animator;
    SpriteRenderer sprite;
    GameObject player;
    [SerializeField] Slider healthSlider;
    // SCRIPTS
    [SerializeField] LevelFadeEffect fadeEffect;
    // Variables
    float health;
    float timer_timeBetweenAttacks = 0;
    bool readyToAttack = false;
    float distanceBetweenPlayerAndBoss;
    bool faceLeft = true; // true == left, false == right
    bool bossCanWalk = false;
    bool bossIsDead = false;

    float Health
    {
        get { return health; }
        set
        {
            if (value == 0) BossStateDeath();
            if (value <= maxHealth)
            {
                if (value <= 40) currentPhase = Phase.second;
                else if (value <= 0) BossStateDeath();
                health = value;
                healthSlider.value = Health;
            }
        }
    }
    enum Phase
    {
        first,
        second
    }
    Phase currentPhase;

    private void Start()
    {
        player = GameObject.Find("Player");
        if (player == null) Debug.Log("!ERROR! SkeletonBossMain could not load Player GameObject");
        fadeEffect.FadeIn();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        healthSlider.maxValue = maxHealth;
        currentPhase = Phase.first;
        animator.SetTrigger("point");
    }

    private void Update()
    {
        if (!bossIsDead)
        {
            // Timers
            if (!readyToAttack)
            {
                timer_timeBetweenAttacks += Time.deltaTime;
                if (timer_timeBetweenAttacks > timeBetweenAttacks)
                {
                    readyToAttack = true;
                    timer_timeBetweenAttacks = 0;
                }
            }
            // Attack and Movement
            distanceBetweenPlayerAndBoss = Vector2.Distance(transform.position, player.transform.position);

            //Boss AI
            if (AnimatorIsPlaying("Point") || AnimatorIsPlaying("Attack")) bossCanWalk = false;
            else bossCanWalk = true;
            //Debug.Log("BossCanWalk: " + bossCanWalk);
            if (bossCanWalk)
            {
                if (readyToAttack && distanceBetweenPlayerAndBoss <= distanceToAttack) BossMelleAttack();
                else if (distanceBetweenPlayerAndBoss <= distanceToIdle) BossStateIdle();
                else if (player.transform.position.x < transform.position.x) BossStateWalkLeft();
                else BossStateWalkRight();
            }
            else rb.velocity = Vector2.zero;
            //Debug.Log("Walk: " + rb.velocity);
        }
    }

    void BossStateIdle()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isWalking", false);
    }
    void BossMelleAttack()
    {
        readyToAttack = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("attack");
    }
    void BossStateDeath()
    {
        animator.SetTrigger("death");
        bossIsDead = true;
        fadeEffect.FadeOut();
    }

    public void BossStateWalkLeft()
    {
        //Debug.Log("BossWalkLeft");
        rb.velocity = Vector2.left * walkingSpeed;
        animator.SetBool("isWalking", true);
        sprite.flipX = false;
    }

    public void BossStateWalkRight()
    {
        //Debug.Log("BossWalkRight");
        rb.velocity = Vector2.right * walkingSpeed;
        animator.SetBool("isWalking", true);
        sprite.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerDamage") health -= 20;  //playerMainScript.Damage();
    }

    public void BossHealAfterDamageToPlayer()
    {
        health += 10;
    }

    //UnityForum
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
