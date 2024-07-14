using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    //[SerializeField] Player Player; --PlayerScript
    [SerializeField] float maxHealth;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float timeToRecoverFromAttack;
    [SerializeField] float health;


    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animator;
    BoxCollider2D collider;
    bool isFacingLeft = false;
    bool recoveringFromAttack = false;
    bool readyToAttack = true;
    bool recoveredFromAttack = false;
    float timerToRecoverFromAttack = 0;

    public bool RecoveringFromAttack
    {
        get { return recoveringFromAttack; }
    }
    public bool ReadyToAttack
    {
        get { return readyToAttack; }
    }
    public bool RecoveredFromAttack
    {
        get { return recoveredFromAttack; }
        set { recoveredFromAttack = value; }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        health = maxHealth;
    }
    void Update()
    {
        if (health <= 0) SkeletonDeath();
        if (!recoveringFromAttack) readyToAttack = true;
        else
        {
            timerToRecoverFromAttack += Time.deltaTime;
            if (timerToRecoverFromAttack > timeToRecoverFromAttack)
            {
                timerToRecoverFromAttack = 0;
                recoveringFromAttack = false;
                recoveredFromAttack = true;
            }
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
        recoveringFromAttack = true;
        readyToAttack = false;
    }

    private void SkeletonDeath()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            health -= 50; //weaponDamage;
        }
    }

    public void ChangeFacingDirection()
    {
        if (isFacingLeft) FaceRight();
        else FaceLeft();
    }

    public void ChangeFacingDirection(bool faceLeft)
    {
        if (faceLeft) FaceLeft();
        else FaceRight();
    }

    private void FaceRight()
    {
        isFacingLeft = false;
        animator.SetBool("isFacingLeft", false);
    }
    private void FaceLeft()
    {
        isFacingLeft = true;
        animator.SetBool("isFacingLeft", true);
    }

}
