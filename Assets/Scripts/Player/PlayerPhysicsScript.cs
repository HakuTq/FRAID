using System.Linq;
using UnityEngine;

public class PlayerPhysicsScript : MonoBehaviour
{
    //********** PRIVATE **********
    //***** References *****
    private PlayerMovementScript movementScript;
    private Rigidbody2D rb;
    private Collider2D feet;
    //***** Movement speeds *****
    private int movementSpeedAir = 3;
    private int movementSpeedGround = 5;
    private int jumpHeight = 5;
    //***** States *****
    private bool isTouchingGround
    {
        get
        {
            return feet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        }
    }
    //********** PUBLIC **********
    public bool IsTouchingGround
    {
        get { return isTouchingGround; }
    }

    private void Awake()
    {
        movementScript = FindObjectOfType<PlayerMovementScript>();
        rb = GetComponent<Rigidbody2D>();
        feet = GetComponentsInChildren<Collider2D>().FirstOrDefault(x => x.name == "Feet");
    }

    private void FixedUpdate()
    {
        Vector2 movementVector = rb.velocity;
        if (isTouchingGround)
        {
            if (movementScript.ShouldMove)
            {
                movementVector = movementScript.MoveVector * movementSpeedGround;
            }
            else if (movementScript.ShouldJump)
            {
                movementVector.y = jumpHeight;
            }
            else
            {
                movementVector = Vector2.zero;
            }
        }
        else
        {
            if (movementScript.ShouldMove)
            {
                movementVector.x = movementScript.MoveVector.x * movementSpeedAir;
            }
        }
        rb.velocity = movementVector;
    }
}
