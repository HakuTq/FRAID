using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonEnemyAI : MonoBehaviour
{
    [SerializeField] GameObject patrolPointA;
    [SerializeField] GameObject patrolPointB;
    [SerializeField] GameObject player;
    [SerializeField] SkeletonEnemy skeletonEnemy;
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float distanceToAttack;
    [SerializeField] float gizmosSphereRadius;
    private Rigidbody2D rb;
    private bool patrolGoingLeft = true;

    private CurrentPhase phase;
    public enum CurrentPhase
    {
        Idle,
        Patrol,
        Chase,
        Attack
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
                    if (skeletonEnemy.ReadyToFire) phase = CurrentPhase.Chase;
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
            case CurrentPhase.Chase:
                {
                    if (player.transform.position.x < transform.position.x) rb.velocity = Vector2.left * chaseSpeed;
                    if (player.transform.position.x > transform.position.x) rb.velocity = Vector2.right * chaseSpeed;
                    if (Vector2.Distance(player.transform.position, transform.position) <= distanceToAttack && skeletonEnemy.ReadyToFire) phase = CurrentPhase.Attack; //Phase Attack jestli je enemy dost blízko a mùže støílet
                    break;
                }
            case CurrentPhase.Attack:
                {
                    rb.velocity = new Vector2();
                    skeletonEnemy.ShootArrow();
                    phase = CurrentPhase.Idle;
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
