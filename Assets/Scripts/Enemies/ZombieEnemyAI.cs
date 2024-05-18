using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject patrolPointA;
    [SerializeField] GameObject patrolPointB;
    [SerializeField] GameObject player;
    [SerializeField] ZombieEmemy zombieEnemy;
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float attackedSpeed;
    [SerializeField] float distanceToAttack;
    [SerializeField] float gizmosSphereRadius;
    private Rigidbody2D rb;
    private bool patrolGoingLeft = true;

    private CurrentPhase phase;
    public enum CurrentPhase
    {
        Idle,
        Patrol,
        ChaseAttack,
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        phase = CurrentPhase.Patrol;
    }

    void Update()
    {
        switch (phase)
        {
            case CurrentPhase.Idle:
                {
                    rb.velocity = new Vector2();
                    //animator
                    break;
                }
            case CurrentPhase.Patrol:
                {
                    //Logic
                    if (Vector2.Distance(transform.position, patrolPointB.transform.position) < 0.5f) //pointB je vpravo
                    {
                        patrolGoingLeft = false;
                        //animation 
                    }
                    else if (Vector2.Distance(transform.position, patrolPointA.transform.position) < 0.5f) //pointA je vlevo
                    {
                        patrolGoingLeft = true;
                        //animation
                    }
                    else if (!IsPointBetweenPositionX(patrolPointA.transform.position, patrolPointB.transform.position, transform.position)) //mimo patrol
                    {
                        if (Vector2.Distance(patrolPointA.transform.position, transform.position) > Vector2.Distance(patrolPointB.transform.position, transform.position)) patrolGoingLeft = true; //Když je bod A (vpravo) vzdalenejsi nez bod B (vlevo), tak je enemy v pravo a musi jit doleva
                        else patrolGoingLeft = false;
                    }
                    //Where to move
                    if (patrolGoingLeft) rb.velocity = Vector2.left * speed;
                    else rb.velocity = Vector2.right * speed;
                    break;
                }
            case CurrentPhase.ChaseAttack:
                {
                    if (player.transform.position.x < transform.position.x)
                    {
                        if (zombieEnemy.Attacked) rb.velocity = Vector2.left * attackedSpeed;
                        else rb.velocity = Vector2.left * chaseSpeed;
                    }
                    if (player.transform.position.x > transform.position.x)
                    {
                        if (zombieEnemy.Attacked) rb.velocity = Vector2.right * attackedSpeed;
                        else rb.velocity = Vector2.right * chaseSpeed;
                    }
                    if (Vector2.Distance(player.transform.position, transform.position) <= distanceToAttack && zombieEnemy.ReadyToFire)
                    {
                        zombieEnemy.Attack(); //Muže utoèit za pohybu
                    }
                    break;
                }
        }
    }

    private bool IsPointBetweenPositionX(Vector2 pointA, Vector2 pointB, Vector2 point)
    {
        return (Mathf.Min(pointA.x, pointB.x) <= point.x && Mathf.Max(pointA.x, pointB.x) <= point.x);
    }

    private void OnDrawGizmos() //Zobrazi body a caru patrol enemaka v Unity Scene
    {
        Gizmos.DrawSphere(patrolPointA.transform.position, gizmosSphereRadius);
        Gizmos.DrawSphere(patrolPointB.transform.position, gizmosSphereRadius);
        Gizmos.DrawLine(patrolPointA.transform.position, patrolPointB.transform.position);
    }
}
