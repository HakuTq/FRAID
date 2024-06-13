using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysicsScript : MonoBehaviour
{
    //********** PRIVATE **********
    //***** References *****
    private PlayerMovementScript movementScript;
    private Rigidbody2D rb;
    private Collider2D left;
    private Collider2D down;
    private Collider2D right;
    //***** Movement speeds *****
    [Header("Movement Speeds")]
    [SerializeField] private int movementSpeedAir;
    [SerializeField] private int movementSpeedGround;
    [SerializeField] private int jumpHeight;
    //***** Layers *****
    LayerMask groundLayer;
    //***** States *****
    private bool isTouchingGroundDown
    {
        get
        {
            return down.IsTouchingLayers(groundLayer);
        }
    }
    private bool isTouchingGroundLeft
    {
        get
        {
            return left.IsTouchingLayers(groundLayer);
        }
    }
    private bool isTouchingGroundRight
    {
        get
        {
            return right.IsTouchingLayers(groundLayer);
        }
    }
    //********** PUBLIC **********
    public bool IsTouchingGroundDown
    {
        get { return isTouchingGroundDown; }
    }
    public bool IsTouchingGroundLeft
    {
        get { return isTouchingGroundLeft; }
    }
    public bool IsTouchingGroundRight
    {
        get { return isTouchingGroundRight; }
    }

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        movementScript = FindObjectOfType<PlayerMovementScript>();
        rb = GetComponentInParent<Rigidbody2D>();

        GameObject parent = GameObject.Find("Player");
        //a long and goofy way to find componetnts in siblings
        List<Collider2D> collidersInCollidersSibling = parent.transform.Find("Colliders").GetComponentsInChildren<Collider2D>().ToList();
        Debug.Log(collidersInCollidersSibling.ToString());
        foreach (Collider2D collider in collidersInCollidersSibling)
        {
            switch (collider.name)
            {
                case "Feet":
                    {
                        down = collider; break;
                    }
                case "Left Arm":
                    {
                        left = collider; break;
                    }
                case "Right Arm":
                    {
                        right = collider; break;
                    }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 movementVector = rb.velocity;
        if (isTouchingGroundDown)
        {
            if (movementScript.ShouldMove)
            {
                movementVector.x = (movementScript.MoveVector * movementSpeedGround * Time.deltaTime).x;
            }
            if (movementScript.ShouldJump)
            {
                movementVector.y = (movementScript.MoveVector * jumpHeight * Time.deltaTime).y;
            }
            if (!movementScript.ShouldMove && !movementScript.ShouldJump)
            {
                movementVector = Vector2.zero;
            }
        }
        else
        {
            if (movementScript.ShouldMove && !(isTouchingGroundLeft || isTouchingGroundRight))
            {
                movementVector.x = (movementScript.MoveVector * movementSpeedAir * Time.deltaTime).x;
            }
        }
        rb.velocity = movementVector;
    }
}
