using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    [SerializeField] private int dashSpeed;
    [SerializeField] private int jumpHeight;
    //***** Layers *****
    private LayerMask groundLayer;
    //***** Dashinng *****
    private bool isDashing;
    private Vector2 preDashVelocity;
    private float dashTimeStarted;
    [Header("Dash Time")]
    [SerializeField] private float dashTime;
    //***** Flipping Sprite + animation *****
    private Transform parentTransform;
    [Header("References")]
    [SerializeField] private Animator animator = null;
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
        isDashing = false;

        GameObject parent = GameObject.Find("Player");

        parentTransform = parent.GetComponent<Transform>();
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

    private void Update()
    {
        animator.SetBool("ShouldJump", movementScript.ShouldJump);
        animator.SetBool("ShouldMove", movementScript.ShouldMove);
        animator.SetBool("IsTouchingGround", IsTouchingGroundDown);

        if (rb.velocity.x > 0)
        {
            ScaleX(parentTransform, false);
        }
        else if (rb.velocity.x < 0)
        {
            ScaleX(parentTransform, true);
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
            if (movementScript.ShouldDash)
            {
                if (!isDashing)
                {
                    isDashing = true;
                    preDashVelocity = rb.velocity;
                    dashTimeStarted = Time.time;
                    movementVector.x = (movementScript.MoveVector * dashSpeed * Time.deltaTime).x;
                }

                if (Time.time - dashTimeStarted > dashTime)
                {
                    movementScript.ShouldDash = false;
                    isDashing = false;
                    movementVector = preDashVelocity;
                }
            }
            else if (movementScript.ShouldMove && !(isTouchingGroundLeft || isTouchingGroundRight))
            {
                movementVector.x = (movementScript.MoveVector * movementSpeedAir * Time.deltaTime).x;
            }
        }
        rb.velocity = movementVector;
    }

    private void ScaleX(Transform transform, bool isXPositive)
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        if (isXPositive)
        {
            transform.localScale = new Vector2(+math.abs(x), y);
        }
        else
        {
            transform.localScale = new Vector2(-math.abs(x), y);
        }
    }
}
