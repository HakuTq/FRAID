using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    //[SerializeField] Player Player; --PlayerScript
    [SerializeField] float speedOfWalking;
    [SerializeField] float maxHealth;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float nextFireTime;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    BoxCollider2D collider;
    float timerFired = 0;
    bool readyToFire = true;
    double health;

    public bool ReadyToFire
    {
        get { return readyToFire; }
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
        if (!readyToFire)
        {
            timerFired += Time.deltaTime;
            if (timerFired > nextFireTime)
            {
                readyToFire = true;
                timerFired = 0;
            }
        }
    }

    public void StartWalking(bool walkLeft)
    {
        //animations
        if (walkLeft) rb.velocity = Vector2.left * speedOfWalking;
        else rb.velocity = Vector2.right * speedOfWalking;
    }

    public void StopWalking()
    {
        rb.velocity = Vector3.zero;
    }

    public void Turn(bool turnLeft)
    {
        if (turnLeft) sprite.flipX = true;
        else sprite.flipX = false;
    }

    public void ShootArrow()
    {
        Instantiate(arrowPrefab, transform.position, transform.rotation);
        readyToFire = false;
    }

    //public void DistanceBetweenEnemyAndPlayerX(out float x, out float y)
    //{
    //    x = Math.Abs(PlayerPositionX - transform.position.x);
    //    y = Math.Abs(PlayerPositionY - transform.position.y);
    //}

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
