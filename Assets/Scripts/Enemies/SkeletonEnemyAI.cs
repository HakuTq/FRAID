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
    [SerializeField] float speed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float distanceToAttack;
    [SerializeField] float gizmosSphereRadius;    
    [SerializeField] float distanceToPatrol;
    //Unity Shit
    Rigidbody2D rb;
    //ma shit
    bool patrolGoingLeft = true;
    float timeToChangeFacingDirection;
    float timerChangeFacingDirection = 0;
    float timerToRecoverFromAttack = 0;
    float distanceBetweenPlayerAndEntity;

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
        timeToChangeFacingDirection = (Random.value * 10) + 10; //min = 10s, max = 20
    }

    void Update()
    {
        distanceBetweenPlayerAndEntity = Vector2.Distance(player.transform.position, transform.position);
        switch (phase)
        {
            case CurrentPhase.Idle: //Skeleton is Still, changes facing direction every (10,20) seconds
                {
                    rb.angularVelocity = 0;
                    if (timerChangeFacingDirection > timeToChangeFacingDirection)
                    {
                        timerChangeFacingDirection = 0;
                        skeletonEnemy.ChangeFacingDirection();
                    }
                    else
                    {
                        timerChangeFacingDirection += Time.deltaTime;
                    }
                    break;
                }
            case CurrentPhase.Patrol: //Patrols between two points
                {
                    //Logic
                    if (Vector2.Distance(transform.position, patrolPointB.transform.position) < 0.5f) //pointB je vpravo
                    {
                        patrolGoingLeft = false;
                    }
                    else if (Vector2.Distance(transform.position, patrolPointA.transform.position) < 0.5f) //pointA je vlevo
                    {
                        patrolGoingLeft = true;
                    }
                    else if (!IsPointBetweenPositionX(patrolPointA.transform.position, patrolPointB.transform.position, transform.position)) //mimo patrol
                    {
                        if (Vector2.Distance(patrolPointA.transform.position, transform.position) > Vector2.Distance(patrolPointB.transform.position, transform.position)) patrolGoingLeft = true; //Když je bod A (vpravo) vzdalenejsi nez bod B (vlevo), tak je enemy v pravo a musi jit doleva
                        else patrolGoingLeft = false;
                    }
                    //animator based on patrolGoingLeft
                    //Where to move
                    if (patrolGoingLeft) rb.velocity = Vector2.left * speed;
                    else rb.velocity = Vector2.right * speed;
                    break;
                }
            case CurrentPhase.Chase: //Chases the Player
                {
                    if (distanceBetweenPlayerAndEntity >= distanceToPatrol) phase = CurrentPhase.Patrol;
                    else
                    {
                        if (player.transform.position.x < transform.position.x) rb.velocity = Vector2.left * chaseSpeed;
                        if (player.transform.position.x > transform.position.x) rb.velocity = Vector2.right * chaseSpeed;
                        if (distanceBetweenPlayerAndEntity <= distanceToAttack && skeletonEnemy.ReadyToAttack) phase = CurrentPhase.Attack; //Goes to attack Phase if close enough to the player
                    }
                    break;
                }
            case CurrentPhase.Attack: //Attack is triggered if Skeleton is Close Enough, After the attack, the unit is stationary for a while before going into the chase phase
                {
                    if (skeletonEnemy.RecoveredFromAttack)
                    {
                        skeletonEnemy.RecoveredFromAttack = false;
                        phase = CurrentPhase.Chase;
                    }
                    else
                    {
                        if (skeletonEnemy.ReadyToAttack && !skeletonEnemy.RecoveringFromAttack) skeletonEnemy.Attack();
                    }
                    break;
                }
        }
    }

    bool IsPointBetweenPositionX(Vector2 pointA, Vector2 pointB, Vector2 point)
    {
        return (Mathf.Min(pointA.x, pointB.x) <= point.x && Mathf.Max(pointA.x, pointB.x) <= point.x);
    }

    void OnDrawGizmos() //Zobrazi body a caru patrol enemaka v Unity Scene
    {
        Gizmos.DrawSphere(patrolPointA.transform.position, gizmosSphereRadius);
        Gizmos.DrawSphere(patrolPointB.transform.position, gizmosSphereRadius);
        Gizmos.DrawLine(patrolPointA.transform.position, patrolPointB.transform.position);
    }
}
