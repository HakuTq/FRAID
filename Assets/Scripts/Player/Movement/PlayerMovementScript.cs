using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    //********** PRIVATE **********

    private Vector2 moveVector;
    private bool shouldMove;
    private bool shouldJump;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private Animator animator = null;

    //********** PUBLIC **********

    public Vector2 MoveVector { get { return moveVector; } }
    public bool ShouldMove { get { return shouldMove;} }
    public bool ShouldJump { get { return shouldJump;} }
    public PlayerInput PlayerInput => playerInput;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initializes variables
        moveVector = Vector2.zero;
        shouldMove = false;
        shouldJump = false;
    }

    private void Update()
    {
        animator.SetBool("PlayerJump", ShouldJump);
        animator.SetBool("PlayerRun", shouldMove);
    }

    // Called when the "GoLeft" action is triggered
    public void GoLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveVector.x = -1;
            shouldMove = true;
        }
        else if (context.canceled && moveVector.x == -1)
        {
            moveVector.x = 0;
            shouldMove = false;
        }
    }

    // Called when the "GoRight" action is triggered
    public void GoRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveVector.x = 1;
            shouldMove = true;
        }
        else if (context.canceled && moveVector.x == 1)
        {
            moveVector.x = 0;
            shouldMove = false;
        }
    }

    // Called when the "Jump" action is triggered
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Set moveVector.y to 1 when the action is started (jumping up)
            moveVector.y = 1;
            shouldJump = true;
        }
        else if (context.canceled)
        {
            moveVector.y = 0;
            shouldJump = false;
        }
    }
}
