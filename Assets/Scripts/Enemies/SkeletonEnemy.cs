using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    //[SerializeField] Player Player; --PlayerScript
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

    public void ShootArrow()
    {
        Instantiate(arrowPrefab, transform.position, transform.rotation);
        readyToFire = false;
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
