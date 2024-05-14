using UnityEngine;

public class PlayerPhysicsScript : MonoBehaviour
{
    //********** PRIVATE **********
    //***** References *****
    private PlayerMovementScript movementScript;
    private Rigidbody2D rb;
    //***** Movement speeds *****
    private int movementSpeed = 5;
    private int jumpHeight = 10;
    //********** PUBLIC **********
    private void Awake()
    {
        movementScript = FindObjectOfType<PlayerMovementScript>();
        rb = FindObjectOfType<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (movementScript.ShouldMove)
        {
            rb.velocity = movementScript.MoveVector * movementSpeed;
        }
        else if(movementScript.ShouldJump)
        {
            rb.velocity = movementScript.MoveVector * jumpHeight;
        }
    }
}
